using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using Quartz;
using StackExchange.Redis;
using ThreadLike.Common.Application.Caching;
using ThreadLike.Common.Application.Data;
using ThreadLike.Common.Application.EventBus;
using ThreadLike.Common.Domain;
using ThreadLike.Common.Infrastructure.Authentication;
using ThreadLike.Common.Infrastructure.Authorization;
using ThreadLike.Common.Infrastructure.Caching;
using ThreadLike.Common.Infrastructure.Data;
using ThreadLike.Common.Infrastructure.EventBuses;
using ThreadLike.Common.Infrastructure.Outbox;
using ThreadLike.Common.Infrastructure.Repositories;

namespace ThreadLike.Common.Infrastructure;

public static class InfrastructureConfiguration
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services,
		IConfiguration configuration,
		RabbitMqSettings rabbitMqSettings,
		string serviceName,
		Action<IRegistrationConfigurator, string>[] eventConsumerRegistration)
	{
		string? databaseConnectionString = configuration.GetConnectionString("Database")!;
		string? cacheConnectionString = configuration.GetConnectionString("Cache");
		//string messaggeQueueConnectionString = configuration.GetConnectionString("Queue");
		//------------------------------- Auth section -------------------------------
		services.AddAuthenticationInternal(configuration);
		//------------------------------- Auth section -------------------------------
		//------------------------------- Authorization section -------------------------------
		services.AddAuthorizationInternal();
		//------------------------------- Authorization section -------------------------------

		NpgsqlDataSource dataSource = new NpgsqlDataSourceBuilder(databaseConnectionString).Build();
		services.AddSingleton(dataSource);
		services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

		//services.AddDistributedMemoryCache(setup =>
		//{
		//});
		services.TryAddSingleton<ICacheService, CacheService>();
		IConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(cacheConnectionString, config =>
		{
			// this only be places here for ONE PURPOSE ( SHOULD BE REMOVED IN PROD  )
			//  --------------------- FOR MIGRATION PURPOSE ----------------------------
			// without this migration  can't build project
			config.AbortOnConnectFail = false;
			config.BeforeSocketConnect = (endpoint, connectionType, socket) =>
			{
				Console.WriteLine($"endpoint:{endpoint.AddressFamily.ToString()}; type:{connectionType.ToString()}; remote_endpoint:{socket.RemoteEndPoint?.AddressFamily.ToString()}");
			};
		});
		services.TryAddSingleton(connectionMultiplexer);
		services.AddStackExchangeRedisCache(opt =>
		{
			opt.ConnectionMultiplexerFactory = () => Task.FromResult(connectionMultiplexer);
		});

		//register interceptors
		services.AddSingleton<InsertOutboxMessageEventsInterceptor>();


		//------------------------------- Event buss section -------------------------------
		// add Event bus
		services.TryAddSingleton<IEventBus, EventBus>();
		// add masstransit
		services.AddMassTransit(config =>
		{
			string instanceId = serviceName.ToLowerInvariant().Replace(".", "-");

			foreach (Action<IRegistrationConfigurator, string> moduleConsumerRegister in eventConsumerRegistration)
			{
				moduleConsumerRegister(config, instanceId);
			}

			config.SetKebabCaseEndpointNameFormatter();

			config.UsingRabbitMq((ctx, cfg) =>
			{
				cfg.Host(new Uri(rabbitMqSettings.Host), config =>
				{
					config.Username(rabbitMqSettings.Username);
					config.Password(rabbitMqSettings.Password);
				});
				cfg.ConfigureEndpoints(ctx);
			});
		});
		//------------------------------- Event buss section -------------------------------



		//------------------------------- QUARTZ for BG Job -------------------------------//
		services.AddQuartz(configurator =>
		{
			var scheduler = Guid.NewGuid();
			configurator.SchedulerId = $"default-id-{serviceName}";
			configurator.SchedulerName = $"default-name-{serviceName}";
		});
		services.AddQuartzHostedService(options =>
		{
			options.WaitForJobsToComplete = true;
		});
		//------------------------------- QUARTZ for BG Job -------------------------------//




		//------------------------------- OpenTelemetry SERVICE -------------------------------//
		//services
		//	.AddOpenTelemetry()
		//	.ConfigureResource(resource => resource.AddService(serviceName))
		//	.WithTracing(tracing =>
		//	{
		//		tracing
		//			.AddAspNetCoreInstrumentation(config =>
		//			{

		//			})
		//			.AddHttpClientInstrumentation()
		//			.AddEntityFrameworkCoreInstrumentation()
		//			.AddRedisInstrumentation(connectionMultiplexer)
		//			.AddNpgsql(option =>
		//			{
		//				//option.EnableConnectionLevelAttributes = true;
		//				//option.EnableStatementLevelAttributes = true;
		//			})
		//			.AddSource(MassTransit.Logging.DiagnosticHeaders.DefaultListenerName);

		//		tracing.AddOtlpExporter();
		//	});

		// Enable detailed logging for OpenTelemetry and Redis
		return services;
	}
}
