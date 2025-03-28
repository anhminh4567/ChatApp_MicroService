using MassTransit;
using ThreadLike.Chat.Infrastructure.Inbox;
using ThreadLike.Common.Application.Data;
using ThreadLike.Common.Contracts;

namespace ThreadLike.Chat.Api.Consumers
{
	public class UserUpdatedIntegrationEventConsumer : IntegrationEventConsumer<UsersModuleContracts.UserUpdatedIntegrationEvent>
	{
		public UserUpdatedIntegrationEventConsumer(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory)
		{
		}
	}

}
