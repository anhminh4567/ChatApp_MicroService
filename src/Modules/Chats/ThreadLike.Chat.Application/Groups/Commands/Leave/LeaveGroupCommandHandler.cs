using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Application.Abstracts;
using ThreadLike.Chat.Domain.Groups;
using ThreadLike.Chat.Domain.Groups.Entities;
using ThreadLike.Chat.Domain.Users;
using ThreadLike.Common.Application.Messaging;
using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.Application.Groups.Commands.Leave
{
	public record LeaveGroupCommand(Guid GroupId, string UserId) : ICommand<Result>;
	internal class LeaveGroupCommandHandler(
		IGroupRepository groupRepository,
		IUserRepository userRepository,
		IUnitOfWork	unitOfWork) : ICommandHandler<LeaveGroupCommand, Result>
	{
		public async Task<Result> Handle(LeaveGroupCommand request, CancellationToken cancellationToken)
		{
			Group? group = await groupRepository.GetById(request.GroupId, cancellationToken);

			if (group == null)
				return Result.Failure(GroupErrors.NotFound);
			
			User? user = await userRepository.GetById(request.UserId, cancellationToken);
			if (user == null)
				return Result.Failure(UserErrors.NotFound);

			Participant? participant = group.Participants.FirstOrDefault(x => x.UserId == user.Id);
			
			if (participant == null)
				return Result.Failure(GroupErrors.ParticipantErrors.NotFound);
			
			group.RemoveFromGroup(user);

			groupRepository.Update(group);
			
			await unitOfWork.SaveChangesAsync(cancellationToken);

			return Result.Ok();
		}
    }
}
