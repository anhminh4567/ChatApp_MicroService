using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Common.Application.Data;
using ThreadLike.Common.Application.EventBus;
using ThreadLike.Common.Application.Messaging;
using ThreadLike.User.Domain.Users;
using ThreadLike.User.Domain.Users.DomainEvents;

namespace ThreadLike.User.Application.Users
{
	internal class UserCreatedDomainEventHandler(
		IDbConnectionFactory _dbConnectionFactory,
		IUserRepository _userRepository,
		IEventBus _eventBus 
		) : INotificationHandler<UserCreatedDomainEvent>
	{
		public Task Handle(UserCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
		{
			Console.WriteLine("asfd");
			throw new NotImplementedException();
		}
	}
}
