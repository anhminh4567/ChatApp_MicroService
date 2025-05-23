﻿using Azure.Storage.Blobs;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using StackExchange.Redis;
using System.Reflection;
using ThreadLike.Chat.Application;
using ThreadLike.Chat.Application.Abstracts;
using ThreadLike.Chat.Domain.GroupRoles;
using ThreadLike.Chat.Domain.Groups;
using ThreadLike.Chat.Domain.Messages;
using ThreadLike.Chat.Domain.Reactions;
using ThreadLike.Chat.Domain.Users;
using ThreadLike.Chat.Infrastructure.Authorization;
using ThreadLike.Chat.Infrastructure.Database;
using ThreadLike.Chat.Infrastructure.GroupRoles;
using ThreadLike.Chat.Infrastructure.Groups;
using ThreadLike.Chat.Infrastructure.Inbox;
using ThreadLike.Chat.Infrastructure.Messages;
using ThreadLike.Chat.Infrastructure.Options;
using ThreadLike.Chat.Infrastructure.Outbox;
using ThreadLike.Chat.Infrastructure.Reactions;
using ThreadLike.Chat.Infrastructure.Services;
using ThreadLike.Chat.Infrastructure.Users;
using ThreadLike.Common.Application.Authorization;
using ThreadLike.Common.Application.EventBus;
using ThreadLike.Common.Infrastructure.Authentication;
using ThreadLike.Common.Infrastructure.Outbox;
namespace ThreadLike.Chat.Infrastructure
{
	public static class InfrastructureConfiguration
	{
		private static readonly Assembly _assembly = typeof(InfrastructureConfiguration).Assembly;
		private static readonly Assembly _applicationAssembly = typeof(ApplicationConfiguration).Assembly;
		public static IServiceCollection AddChatInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
		{
			services.AddDbContext<ChatDbContext>((sp, options) =>
			{
				options.UseNpgsql(
					configuration.GetConnectionString("Database"),
					options =>
					{
						options.MigrationsHistoryTable(HistoryRepository.DefaultTableName, ChatDbContext.Schema);
					})
				.AddInterceptors(sp.GetRequiredService<InsertOutboxMessageEventsInterceptor>());
			});

			services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ChatDbContext>());

			services.AddScoped<IAuthorizationService, AuthorizationService>();

			services.AddScoped<IGroupRoleRepository, GroupRoleRepository>();
			services.AddScoped<IGroupRepository, GroupRepository>();
			services.AddScoped<IMessageRepository, MessageRepository>();
			services.AddScoped<IReactionRepository, ReactionRepository>();
			services.AddScoped<IUserRepository, UserRepository>();

			services.AddScoped<IGroupChatService, GroupChatService>();

			services.AddInboxOutbox(configuration);

			services.DecorateDomainEventHandlers(configuration);

			services.DecorateIntegrationEventHandlers(configuration);

			services.AddConfigureOptions(configuration);

			services.AddRealTimeCommunication(configuration,environment);

			services.AddBlobFileStorage(configuration);

			return services;

		}
		private static void AddBlobFileStorage(this IServiceCollection services, IConfiguration configuration)
		{
			//const string BLOB_CONNECTION_STRING_ENV = "CHATAPP_AZURE_BLOB_CONNECTION_STRING";

			//string? connectionString = Environment.GetEnvironmentVariable(BLOB_CONNECTION_STRING_ENV,EnvironmentVariableTarget.User);

			//ArgumentException.ThrowIfNullOrEmpty(connectionString, "Blob storage connection string is required for file storage");

			services.Configure<AzureBlobStorageOptions>(configuration.GetSection(AzureBlobStorageOptions.SectionName));

			services.PostConfigure<AzureBlobStorageOptions>(options =>
			{
				Console.WriteLine(options.ConnectionString);
			});

			services.AddSingleton((IServiceProvider serviceProvider) =>
			{
				var getOption = serviceProvider.GetRequiredService<IOptions<AzureBlobStorageOptions>>();
				if (getOption is null)
					throw new ArgumentNullException();
				AzureBlobStorageOptions option = getOption.Value;
				var newClient = new BlobServiceClient(option.ConnectionString, new BlobClientOptions() 
				{
					Retry =
					{
						MaxRetries = 2,
						Mode = Azure.Core.RetryMode.Fixed,
						Delay = TimeSpan.FromSeconds(5),
						
					}
				});

				return newClient;
			});
			services.AddScoped<IFilesStorageService, AzureBlobFileStorageService>();
		}
		private static void AddRealTimeCommunication(this IServiceCollection services,IConfiguration configuration, IWebHostEnvironment environment)
		{
			string? redisConnectionString = configuration.GetConnectionString("Cache");
			ArgumentNullException.ThrowIfNull(redisConnectionString, "Redis connection string is required for real-time communication");
			Microsoft.AspNetCore.SignalR.ISignalRServerBuilder signalRConfiguration =
				services.AddSignalR()
				.AddJsonProtocol((opt) =>
				{
					opt.PayloadSerializerOptions.PropertyNamingPolicy = null;
				});

			if (environment.IsDevelopment())
			{
				signalRConfiguration.AddStackExchangeRedis(redisConnectionString, options =>
				{
					options.Configuration.ChannelPrefix = RedisChannel.Pattern("signalr");
					options.Configuration.DefaultDatabase = 1;
				});
				//string? connectionString = configuration.GetConnectionString("BackPlane");

				//using (SqlConnection connection = new SqlConnection(connectionString))
				//{
				//	try
				//	{
				//		connection.Open();
				//	}
				//	catch (SqlException ex)
				//	{
				//		// Log the exception details for troubleshooting
				//		Console.WriteLine($"Error connecting to SQL Server: {ex.Message}");
				//	}
				//}
				////to see how they handle shit
				//signalRConfiguration.AddSqlServer(o =>
				//{
				//	ArgumentException.ThrowIfNullOrWhiteSpace(connectionString, "Database connection string is required for SQL Server backplane");

				//	o.ConnectionString = connectionString;
				//	// See above - attempts to enable Service Broker on the database at startup
				//	// if not already enabled. Default false, as this can hang if the database has other sessions.
				//	o.AutoEnableServiceBroker = true;
				//	// Every hub has its own message table(s). 
				//	// This determines the part of the table named that is derived from the hub name.
				//	// IF THIS IS NOT UNIQUE AMONG ALL HUBS, YOUR HUBS WILL COLLIDE AND MESSAGES MIX.
				//	o.TableSlugGenerator = hubType => hubType.Name;
				//	// The number of tables per Hub to use. Adding a few extra could increase throughput
				//	// by reducing table contention, but all servers must agree on the number of tables used.
				//	// If you find that you need to increase this, it is probably a hint that you need to switch to Redis.
				//	o.TableCount = 2;
				//	// The SQL Server schema to use for the backing tables for this backplane.
				//	o.SchemaName = "SignalRCore";
				//});
			}
			else
			{
				signalRConfiguration.AddStackExchangeRedis(redisConnectionString, options =>
				{
					options.Configuration.ChannelPrefix = RedisChannel.Pattern("signalr");
					options.Configuration.DefaultDatabase = 1;
				});
			}

		}
		private static void AddInboxOutbox(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<OutboxOptions>(configuration.GetSection(OutboxOptions.SectionName));
			services.ConfigureOptions<ConfigureProcessOutboxJob>();

			services.Configure<InboxOptions>(configuration.GetSection(InboxOptions.SectionName));
			services.ConfigureOptions<ConfigureProcessInboxJob>();
		}
		private static void AddConfigureOptions(this IServiceCollection services,IConfiguration configuration)
		{
			services.ConfigureOptions<JwtBearerConfigurationForSignalR>();
		}

		private static void DecorateDomainEventHandlers(this IServiceCollection services, IConfiguration configuration)
		{
			Type[] domainEventHandlers = _applicationAssembly
				.GetTypes()
				.Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(INotificationHandler<>)))
				.ToArray();

			foreach (Type handler in domainEventHandlers)
			{

				Type domainEventFromHandler = handler
					.GetInterfaces()
					.First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(INotificationHandler<>))
					.GetGenericArguments()
					.First();

				services.TryAddTransient(typeof(INotificationHandler<>).MakeGenericType(domainEventFromHandler), handler);

				Type idempotentHandler = typeof(IdempotentDomainEventHandler<>).MakeGenericType(domainEventFromHandler);

				services.Decorate(typeof(INotificationHandler<>).MakeGenericType(domainEventFromHandler), idempotentHandler);
			}
		}
		private static void DecorateIntegrationEventHandlers(this IServiceCollection services, IConfiguration configuration)
		{
			
			Type[] integrationEventHandlers = _applicationAssembly
			  .GetTypes()
			  .Where(t => t.IsAssignableTo(typeof(IIntegrationEventHandler)))
			  .ToArray();

			foreach (Type integrationEventHandler in integrationEventHandlers)
			{
				services.TryAddScoped(integrationEventHandler);

				Type integrationEvent = integrationEventHandler
					.GetInterfaces()
					.Single(i => i.IsGenericType)
					.GetGenericArguments()
					.Single();

				Type closedIdempotentHandler =
					typeof(IdempotentIntegrationEventHandler<>).MakeGenericType(integrationEvent);

				services.Decorate(integrationEventHandler, closedIdempotentHandler);
			}
		}
	}
}
