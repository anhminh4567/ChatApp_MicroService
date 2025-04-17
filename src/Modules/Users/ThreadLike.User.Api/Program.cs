using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using ThreadLike.Common.Api;
using ThreadLike.Common.Application;
using ThreadLike.Common.Infrastructure;
using ThreadLike.Common.Infrastructure.EventBuses;
using ThreadLike.User.Api;
using ThreadLike.User.Api.Configures;
using ThreadLike.User.Api.Consumers;
using ThreadLike.User.Api.Endpoints;
using ThreadLike.User.Api.Middlewares;
using ThreadLike.User.Application;
using ThreadLike.User.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddAndConfigureLogging(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();

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
builder.Services.AddApplication([typeof(ThreadLike.User.Application.ApplicationConfiguration).Assembly]);
builder.Services.AddUserApplication();
// Add infra
builder.Services.AddApiModule(builder.Configuration);

builder.Services.AddInfrastructure(builder.Configuration, rabbitMqSettings, UserModuleMetaData.ServiceName,
	[(config, instanceId) => 
	{ 
		config.AddConsumer<GetUserRolesRequestConsummer>().Endpoint(e => e.InstanceId = instanceId ); 
	}]);
builder.Services.AddUserInfrastructure(builder.Configuration);
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
	options.SerializerOptions.PropertyNameCaseInsensitive = false;
	options.SerializerOptions.PropertyNamingPolicy = null;
});

var app = builder.Build();

// Configure the HTTP request pipeline.


if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.UseDeveloperExceptionPage();
}
// use static file have alot of noise so it should not be logged, and placed beefore useSeriologRequestLogging

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

RouteGroupBuilder apiGroup = app.MapGroup("api")
	.WithOpenApi()
	.DisableAntiforgery();

apiGroup.MapUserEndpoints();
apiGroup.MapRoleEndpoints();

app.Run();


