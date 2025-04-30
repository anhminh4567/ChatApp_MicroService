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
using ThreadLike.Chat.Infrastructure.Options;
using ThreadLike.Chat.Infrastructure.Hubs;
using ThreadLike.Common.Domain.Shares;
using Newtonsoft.Json.Serialization;
using System.Text.Json;
using ThreadLike.Common.Api;
using OpenTelemetry.Logs;
using System.Threading.RateLimiting;
internal class Program
{
	private static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);
		// Add LOGGING
		builder.Services.AddLoggingConfig(builder.Configuration);

		builder.AddConfigureOpenTelemetry();
		
		if (builder.Environment.IsDevelopment())
			builder.Configuration.AddJsonFile("appsettings.Secret.Json");

		if (builder.Environment.IsProduction())
			builder.ConfigureProductionSecret();

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
		builder.Services.AddInfrastructure(builder.Environment,builder.Configuration, rabbitMqSettings, ChatModuleMetaData.ServiceName, [
			(config, identity) => {
				config.AddConsumer<UserCreatedIntegrationEventConsumer>((ctx ,cfig) => {
					cfig.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(25)));
				}).Endpoint(cfg => cfg.InstanceId = identity);
				config.AddConsumer<UserUpdatedIntegrationEventConsumer>()
				.Endpoint(cfg => cfg.InstanceId = identity);

			}]);
		builder.Services.AddChatInfrastructure(builder.Configuration,builder.Environment);

		// some service configure for api layer, lilke CORS ( in future, shared middleware )
		builder.Services.AddApiModule(builder.Environment,builder.Configuration);

		builder.Services.AddControllers();
		builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
		{
			options.SerializerOptions.PropertyNameCaseInsensitive = false;
			options.SerializerOptions.PropertyNamingPolicy = null;
		});
		WebApplication app = builder.Build();
	
		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
			app.SeedIcons();
		}
		app.UseSwagger();
		app.UseSwaggerUI();

		app.UseCors(CorsPolicy.AllowClientSPA);

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
		
		app.MapHub<GroupChatHub>($"{JwtBearerConfigurationForSignalR.HubStartSegment}/group-chat");

		app.MapPrometheusScrapingEndpoint();

		app.Run();
	}
}