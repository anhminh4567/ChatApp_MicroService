using MassTransit;
using MediatR;
using ThreadLike.Chat.Infrastructure.Inbox;
using ThreadLike.Common.Application.Data;
using ThreadLike.Common.Contracts;

namespace ThreadLike.Chat.Api.Consumers
{
	public class UserCreatedIntegrationEventConsumer : IntegrationEventConsumer<UsersModuleContracts.UserCreatedIntegrationEvent>
	{
		public UserCreatedIntegrationEventConsumer(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory)
		{
		}
	}
}
