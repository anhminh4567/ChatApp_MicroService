using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Application.Abstracts;
using ThreadLike.Chat.Application.Users.Commands.Create;
using ThreadLike.Chat.Domain.Users;
using ThreadLike.Common.Application.EventBus;
using ThreadLike.Common.Application.Exceptions;
using ThreadLike.Common.Contracts;
using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.Application.Users.ItegrationHandlers
{
	internal class UserCreatedIntegrationEventHandler(
		IMediator mediator) : IntegrationEventHandler<UsersModuleContracts.UserCreatedIntegrationEvent>
	{
		public override async Task Handle(UsersModuleContracts.UserCreatedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
		{
			Result<User> result = await mediator.Send(new CreateUserCommand(integrationEvent.Name, integrationEvent.Email, integrationEvent.IdentityId, integrationEvent.IsVerified));

			if (result.IsFailure)
				throw new TheadlikeApplicationException(nameof(UserCreatedIntegrationEventHandler),result.Error);
		}
	}
}
