using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Application.Abstracts;
using ThreadLike.Chat.Domain.Groups;
using ThreadLike.Chat.Domain.Messages;
using ThreadLike.Chat.Domain.Messages.Entities;
using ThreadLike.Chat.Domain.Reactions;
using ThreadLike.Chat.Domain.Users;
using ThreadLike.Common.Application.Messaging;
using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.Application.Messages.Commands.React
{
	public record ReactToMessageCommand(Guid GroupId , Guid MessageId, string ReactionId, string ReactorId) : ICommand<Result<MessageReaction>>;
	internal class ReactToMessageCommandHandler(
		IMessageRepository messageRepository,
		IReactionRepository reactionRepository,
		IUnitOfWork unitOfWork) : ICommandHandler<ReactToMessageCommand, Result<MessageReaction>>
	{
		public async Task<Result<MessageReaction>> Handle(ReactToMessageCommand request, CancellationToken cancellationToken)
		{
			Message? message = await messageRepository.GetByIdAndGroup(request.MessageId, request.GroupId, cancellationToken);

			if (message == null)
				return Result.Failure(MessageErrors.NotFound);
			
			List<Reaction> reactions = await reactionRepository.GetAll(cancellationToken);

			Reaction? reaction = reactions.FirstOrDefault(x => x.Id == request.ReactionId);
			if(reaction == null)
				return Result.Failure(MessageErrors.ReactionErrors.NotFound);

			MessageReaction? messageReaction = message.MessageReactions.FirstOrDefault(x => x.ReactionId == request.ReactionId);

			if(messageReaction != null)
			{
				messageReaction = MessageReaction.Create(request.MessageId, reaction.Id);	
				message.SetReaction(messageReaction);
			}

			string? reactorId = messageReaction!.ReactorIds.FirstOrDefault(x => x == request.ReactorId);	

			if(reactorId != null)
				messageReaction.RemoveReactor(request.ReactorId);
			else
				messageReaction.AddReactor(request.ReactorId);
			
			messageRepository.Update(message);

			await unitOfWork.SaveChangesAsync(cancellationToken);

			return messageReaction;
		}
	}
}
