using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Application.Abstracts;
using ThreadLike.Chat.Domain.Groups;
using ThreadLike.Chat.Domain.Messages;
using ThreadLike.Chat.Domain.Users;
using ThreadLike.Common.Application.Messaging;
using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.Application.Messages.Commands.Remove
{
	public record RemoveMessageFromGroupCommand(Guid MessageId, Guid GroupId, string SenderId) : ICommand<Result>;
	internal class RemoveMessageFromGroupCommandHandler(
		IUserRepository userRepository,
		IGroupRepository groupRepository,
		IMessageRepository messageRepository,
		IUnitOfWork unitOfWork) : ICommandHandler<RemoveMessageFromGroupCommand, Result>
	{
		public async Task<Result> Handle(RemoveMessageFromGroupCommand request, CancellationToken cancellationToken)
		{
			User user = await userRepository.GetById(request.SenderId);
			if (user == null)
				return Result.Failure(UserErrors.NotFound);

			Message? messsage = await messageRepository.GetByIdAndGroup(request.MessageId, request.GroupId);	
			if (messsage == null)
				return Result.Failure(MessageErrors.NotFound);

			if(messsage.SenderId != user.Id)
				return Result.Failure(MessageErrors.NotBelongToSender);

			messsage.Delete();

			messageRepository.Update(messsage);

			await unitOfWork.SaveChangesAsync(cancellationToken);
			
			return Result.Ok();
		}
	}
}
