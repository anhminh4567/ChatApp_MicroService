using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Common.Application.Data;
using ThreadLike.Common.Application.EventBus;
using ThreadLike.Common.Application.Messaging;
using ThreadLike.Common.Contracts;
using ThreadLike.User.Domain.Users;
using ThreadLike.User.Domain.Users.DomainEvents;

namespace ThreadLike.User.Application.Users.DomainHandlers
{
	internal class UserCreatedDomainEventHandler(
		IEventBus eventBus,
		IUserRepository userRepository
		) : INotificationHandler<UserCreatedDomainEvent>
	{
		public async Task Handle(UserCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
		{
			Domain.Users.User getUser  = await userRepository.GetById(domainEvent.NewUserId);
			
			await eventBus.PublishAsync(new UsersModuleContracts.UserCreatedIntegrationEvent(
				domainEvent.Id,
				domainEvent.OccuredTimeUtc,
				getUser.Id,
				getUser.Email,
				getUser.Name,
				getUser.IdentityId,
				getUser.IsVerified));
		}
	}
}
