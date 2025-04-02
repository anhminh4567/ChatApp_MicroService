using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Application.Abstracts;
using ThreadLike.Chat.Domain.GroupRoles;
using ThreadLike.Chat.Domain.Groups;
using ThreadLike.Chat.Domain.Groups.Entities;
using ThreadLike.Chat.Domain.Users;
using ThreadLike.Common.Application.Messaging;
using ThreadLike.Common.Domain;
using FluentValidation;
namespace ThreadLike.Chat.Application.Groups.Commands.Join
{
	public record JoinGroupCommand(Guid GroupId, string UserId, string AdderId) : ICommand<Result<List<Participant>>>;
	internal class JoinGroupCommandHandler(
		IUserRepository userRepository,
		IGroupRepository groupRepository,
		IUnitOfWork unitOfWork) : ICommandHandler<JoinGroupCommand, Result<List<Participant>>>
	{
		public async Task<Result<List<Participant>>> Handle(JoinGroupCommand request, CancellationToken cancellationToken)
		{
			Group? group = await groupRepository.GetById(request.GroupId, cancellationToken);

			if (group == null)
				return Result.Failure(GroupErrors.NotFound);

			User? user = await userRepository.GetById(request.UserId, cancellationToken);
			if (user == null)
				return Result.Failure(UserErrors.NotFound);

			List<Participant> groupLeaders = group.Participants.Where(x => x.RoleName == GroupRole.GroupLeader.Role).ToList();

			if (groupLeaders.Any(x => x.UserId == request.AdderId) == false)
				return Result.Failure(GroupErrors.ParticipantErrors.IsNotGroupLeader(request.AdderId));

			group.AddToGroup(user,GroupRole.Member);

			groupRepository.Update(group);

			await unitOfWork.SaveChangesAsync(cancellationToken);

			return group.Participants;
		}
	}

}
