using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Application.Abstracts;
using ThreadLike.Chat.Domain.GroupRoles;
using ThreadLike.Chat.Domain.Users;
using ThreadLike.Common.Application.Messaging;
using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.Application.Users.Commands.Create
{
	public record CreateUserCommand(string Name, string Email, string IdentityId, bool IsVerified) : ICommand<Result<User>>;
	internal class CreateUserCommandHandler(
		IUserRepository userRepository,
		IUnitOfWork unitOfWork,
		IMediator mediator) : ICommandHandler<CreateUserCommand, Result<User>>
	{
		public async Task<Result<User>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
		{

			User user = User.Create(request.Name, request.Email, request.IdentityId);

			if (request.IsVerified)
			{
				user.Verify();
			}

			userRepository.Create(user);

			await unitOfWork.SaveChangesAsync();

			return user;
		}
	}

}
