using Amazon.CognitoIdentityProvider.Model.Internal.MarshallTransformations;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System.Reflection;
using ThreadLike.Common.Application.Authorization;
using ThreadLike.Common.Infrastructure.Authentication;
using ThreadLike.Common.Infrastructure.Outbox;
using ThreadLike.User.Application;
using ThreadLike.User.Application.Abstractions;
using ThreadLike.User.Application.Abstractions.Identity;
using ThreadLike.User.Domain.Roles;
using ThreadLike.User.Domain.Users;
using ThreadLike.User.Infrastructure.Authorization;
using ThreadLike.User.Infrastructure.Constant;
using ThreadLike.User.Infrastructure.Database;
using ThreadLike.User.Infrastructure.Identity;
using ThreadLike.User.Infrastructure.Inbox;
using ThreadLike.User.Infrastructure.Outbox;
using ThreadLike.User.Infrastructure.Roles;
using ThreadLike.User.Infrastructure.Users;

namespace ThreadLike.User.Infrastructure
{
	public static class InfrastructureConfiguration
	{
		private static readonly Assembly _assembly = typeof(InfrastructureConfiguration).Assembly;
		private static readonly Assembly _applicationAssembly = typeof(ApplicationConfiguration).Assembly;
		public static IServiceCollection AddUserInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<UserDbContext>( (sp,options )=>
			{
				options.UseNpgsql(
					configuration.GetConnectionString("Database"),
					options =>
					{
						options.MigrationsHistoryTable(HistoryRepository.DefaultTableName, UserDbContext.Schema);
					})
				.AddInterceptors(sp.GetRequiredService<InsertOutboxMessageEventsInterceptor>());
			});

			services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<UserDbContext>());

			services.AddScoped<IIdentityProviderService, IdentityProviderService>();

			services.AddScoped<IAuthorizationService, AuthorizationService>();

			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<IRoleRepository, RoleRepository>();

			services.AddIdentityService();

			services.AddModuleHttpClient(configuration);

			services.Configure<OutboxOptions>(configuration.GetSection(OutboxOptions.SectionName));
			services.ConfigureOptions<ConfigureProcessOutboxJob>();

			services.Configure<InboxOptions>(configuration.GetSection(InboxOptions.SectionName));
			services.ConfigureOptions<ConfigureProcessInboxJob>();

			services.DecorateDomainEventHandlers(configuration);
			return services;
		}
		private static IServiceCollection AddIdentityService(this IServiceCollection services)
		{
			//AmazonCognitoIdentityProviderClient cognitoClient = new AmazonCognitoIdentityProviderClient()
			//{
			//	Config =
			//	{
					
			//	}
			//};
			return services;
		}
		public static Action<IBusRegistrationConfigurator, string> ConfigureConsumers(IConfiguration configuration)
		{
			return (configurator, queueName) =>
			{
				
			};
		}
		private static IServiceCollection AddModuleHttpClient(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddHttpClient(NamedHttpClients.AWSCognitoTokenHttpClient,(IServiceProvider sp,HttpClient client ) =>
			{
				AuthenticationOptions authOptions = sp.GetRequiredService<IOptions<AuthenticationOptions>>().Value;

				client.BaseAddress = new Uri(authOptions.TokenEndpoint);
			});
			services.AddHttpClient(NamedHttpClients.AWSCognitoMetadataHttpClient, (IServiceProvider sp, HttpClient client) =>
			{
				AuthenticationOptions authOptions = sp.GetRequiredService<IOptions<AuthenticationOptions>>().Value;

				client.BaseAddress = new Uri(authOptions.MetadataAddress);
			});

			return services;
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

				services.TryAddTransient(typeof(INotificationHandler<>).MakeGenericType(domainEventFromHandler),handler);

				Type idempotentHandler = typeof(IdempotentDomainEventHandler<>).MakeGenericType(domainEventFromHandler);

				services.Decorate(typeof(INotificationHandler<>).MakeGenericType(domainEventFromHandler), idempotentHandler);
			}
		}
	}
}
