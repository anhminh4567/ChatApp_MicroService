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

namespace ThreadLike.Chat.Application.Groups.Commands.CreateGroup
{
	public record CreateGroupCommand(string Name, string CreatorId, List<string> Participants) : ICommand<Result<Group>>;
	internal class CreateGroupCommandHandler(
		IUserRepository userRepository,
		IGroupRepository groupRepository,
		IUnitOfWork unitOfWork): ICommandHandler<CreateGroupCommand, Result<Group>>
	{
		public async Task<Result<Group>> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
		{
			User creator = await userRepository.GetById(request.CreatorId);
			if (creator == null)
				return Result.Failure(UserErrors.NotFound);

			bool isAllExist = await userRepository.Exist(request.Participants,cancellationToken);
			if (!isAllExist)
				return Result.Failure(UserErrors.Create("participants not found","404"));

			List<User> participants = await userRepository.GetByManyIds(request.Participants, cancellationToken);

			Group group = Group.Create(request.Name, creator, GroupType.Group, participants);

			groupRepository.Create(group);
			await unitOfWork.SaveChangesAsync(cancellationToken);
			
			return Result.Ok(group);
		}
	}
}
