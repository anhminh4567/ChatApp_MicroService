using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Application.Abstracts;
using ThreadLike.Chat.Domain.Groups;
using ThreadLike.Chat.Domain.Groups.Enums;
using ThreadLike.Chat.Domain.Users;
using ThreadLike.Common.Application.Messaging;
using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.Application.Groups.Commands.CreatePrivate
{
	public record CreatePrivateGroupCommand(string initiatorId, string friendId) : ICommand<Result<Group>>;

	internal class CreatePrivateGroupCommandHandler(
		IUserRepository userRepository,
		IGroupRepository groupRepository,
		IUnitOfWork unitOfWork) 
		: ICommandHandler<CreatePrivateGroupCommand, Result<Group>>
	{
		public async Task<Result<Group>> Handle(CreatePrivateGroupCommand request, CancellationToken cancellationToken)
		{
			User initiator = await userRepository.GetById(request.initiatorId);
			User friend = await userRepository.GetById(request.friendId);
			
			if(initiator == null || friend == null)
				return Result.Failure(UserErrors.NotFound);

			Group group = Group.Create(Group.PRIVATE_GROUP_NAME, initiator, GroupType.Peer, new List<User> { initiator, friend });

			groupRepository.Create(group);

			await unitOfWork.SaveChangesAsync(cancellationToken);

			return Result.Ok(group);
			
		}
	}
}
