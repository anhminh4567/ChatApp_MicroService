using MassTransit;
using MediatR;
using ThreadLike.Common.Contracts;

namespace ThreadLike.Chat.Api.Consumers
{
	public class UserCreatedIntegrationEventConsumer(IMediator mediator) : IConsumer<UsersModuleContracts.UserCreatedIntegrationEvent>
	{
		public async Task Consume(ConsumeContext<UsersModuleContracts.UserCreatedIntegrationEvent> context)
		{
			throw new NotImplementedException();
		}
	}
}
