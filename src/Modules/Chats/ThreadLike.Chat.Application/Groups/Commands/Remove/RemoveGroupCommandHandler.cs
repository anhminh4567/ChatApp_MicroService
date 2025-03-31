using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Application.Abstracts;
using ThreadLike.Chat.Domain.Groups;
using ThreadLike.Chat.Domain.Users;
using ThreadLike.Common.Application.Messaging;
using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.Application.Groups.Commands.Remove
{
	public record RemoveGroupCommand(Guid GroupId, string CreatorId) : ICommand<Result>;

	internal class RemoveGroupCommandHandler(
		IGroupRepository groupRepository,
		IUserRepository userRepository,
		IUnitOfWork unitOfWork)
		: ICommandHandler<RemoveGroupCommand, Result>
	{
		public async Task<Result> Handle(RemoveGroupCommand request, CancellationToken cancellationToken)
		{
			Group group = await groupRepository.GetById(request.GroupId);
			if (group == null)
				return Result.Failure(GroupErrors.NotFound);

			User user = await userRepository.GetById(request.CreatorId);
			if (user == null)
				return Result.Failure(UserErrors.NotFound);

			Result deleteResult = group.Delete(user);
			if (deleteResult.IsFailure)
				return deleteResult;

			groupRepository.Delete(group);

			await unitOfWork.SaveChangesAsync(cancellationToken);

			return Result.Ok();
		}
	}
}
