using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
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
using ThreadLike.Chat.Infrastructure.Messages;
using ThreadLike.Chat.Infrastructure.Reactions;
using ThreadLike.Chat.Infrastructure.Users;
using ThreadLike.Common.Application.Authorization;
using ThreadLike.Common.Infrastructure.Authentication;
using ThreadLike.Common.Infrastructure.Outbox;


namespace ThreadLike.Chat.Infrastructure
{
	public static class InfrastructureConfiguration
	{
		private static readonly Assembly _assembly = typeof(InfrastructureConfiguration).Assembly;
		private static readonly Assembly _applicationAssembly = typeof(ApplicationConfiguration).Assembly;
		public static IServiceCollection AddChatInfrastructure(this IServiceCollection services, IConfiguration configuration)
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


			return services;
		}
		
		//private static void DecorateDomainEventHandlers(this IServiceCollection services, IConfiguration configuration)
		//{
		//	Type[] domainEventHandlers = _applicationAssembly
		//		.GetTypes()
		//		.Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(INotificationHandler<>)))
		//		.ToArray();

		//	foreach (Type handler in domainEventHandlers)
		//	{

		//		Type domainEventFromHandler = handler
		//			.GetInterfaces()
		//			.First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(INotificationHandler<>))
		//			.GetGenericArguments()
		//			.First();

		//		services.TryAddTransient(typeof(INotificationHandler<>).MakeGenericType(domainEventFromHandler), handler);

		//		Type idempotentHandler = typeof(IdempotentDomainEventHandler<>).MakeGenericType(domainEventFromHandler);

		//		services.Decorate(typeof(INotificationHandler<>).MakeGenericType(domainEventFromHandler), idempotentHandler);
		//	}
		//}
	}
}
