using MassTransit;
using ThreadLike.Common.Contracts;

namespace ThreadLike.Chat.Api.Consumers
{
	public class UserUpdatedIntegrationEventConsumer : IConsumer<UsersModuleContracts.UserUpdatedIntegrationEvent>
	{
		public Task Consume(ConsumeContext<UsersModuleContracts.UserUpdatedIntegrationEvent> context)
		{
			throw new NotImplementedException();
		}
	}

}
