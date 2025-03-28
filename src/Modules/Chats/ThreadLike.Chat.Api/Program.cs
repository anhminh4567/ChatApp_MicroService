using Microsoft.OpenApi.Models;
using Serilog;
using ThreadLike.Chat.Api.Middlewares;
using ThreadLike.Common.Infrastructure;
using ThreadLike.Common.Infrastructure.EventBuses;
using ThreadLike.Common.Application;
using ThreadLike.Chat.Application;
using ThreadLike.Chat.Infrastructure;
using ThreadLike.Chat.Api.Extensions;
using ThreadLike.Chat.Api;
using ThreadLike.Chat.Api.Consumers;
using MassTransit;
internal class Program
{
	private static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		builder.Services.AddSerilog((services, config) =>
		{
			config.ReadFrom.Configuration(builder.Configuration);
		}, false, false);
		builder.Services.AddControllers();
		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();

		builder.Services.AddSwaggerGen(opt =>
		{
			opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
			{
				In = ParameterLocation.Header,
				Description = "Please enter token",
				Name = "Authorization",
				Type = SecuritySchemeType.Http,
				BearerFormat = "JWT",
				Scheme = "bearer"
			});
			opt.AddSecurityRequirement(new OpenApiSecurityRequirement
							{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type=ReferenceType.SecurityScheme,
							Id="Bearer"
						}
					},
					new string[]{}
				}
					});
		});

		builder.Services.AddScoped<CustomExceptionHandlerMiddleware>();
		builder.Services.AddScoped<LogContextTraceLoggingMiddleware>();

		string? rabbitMqHost = builder.Configuration.GetConnectionString("Queue");
		ArgumentException.ThrowIfNullOrEmpty(rabbitMqHost);
		var rabbitMqSettings = new RabbitMqSettings()
		{
			Host = rabbitMqHost,
			Username = "guest",
			Password = "guest"
		};


		// add appliication
		builder.Services.AddApplication([typeof(ThreadLike.Chat.Application.ApplicationConfiguration).Assembly]);
		builder.Services.AddChatApplication();
		// Add infra
		builder.Services.AddInfrastructure(builder.Configuration, rabbitMqSettings, ChatModuleMetaData.ServiceName, [
			(config, identity) => {
				config.AddConsumer<UserCreatedIntegrationEventConsumer>((ctx ,cfig) => {
					cfig.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(25)));
				}).Endpoint(cfg => cfg.InstanceId = identity);
				config.AddConsumer<UserUpdatedIntegrationEventConsumer>()
				.Endpoint(cfg => cfg.InstanceId = identity);

			}]);
		builder.Services.AddChatInfrastructure(builder.Configuration);



		WebApplication app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
			app.UseDeveloperExceptionPage();
			app.SeedIcons();
		}
		app.UseMiddleware<LogContextTraceLoggingMiddleware>();

		app.UseSerilogRequestLogging(options =>
		{
			options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
			{
				diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
				diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
				diagnosticContext.Set("ClientIp", httpContext.Connection.RemoteIpAddress?.MapToIPv4()?.ToString());
			};
		});

		app.UseMiddleware<CustomExceptionHandlerMiddleware>();

		app.UseAuthentication();

		app.UseAuthorization();
		app.MapControllers();

		app.Run();
	}
}