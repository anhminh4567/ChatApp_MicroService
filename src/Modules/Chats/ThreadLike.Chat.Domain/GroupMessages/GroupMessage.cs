using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.GroupMessages.Entities;
using ThreadLike.Chat.Domain.Messages;
using ThreadLike.Chat.Domain.Users;
using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.Domain.GroupMessages
{
	public class GroupMessage : Entity
	{
		public Guid Id { get; private set; }
		public Guid GroupId { get; private set; }
		public string SenderId { get; private set; }
		public DateTime CreatedAt { get; private set; }
		public string Content { get; private set; } = "";
		public Guid? ReferenceId { get; private set; }
		public GroupMessage? RefrenceMessage { get; private set; }
		public List<GroupMessageReaction> Reactions { get; private set; } = new();

		private GroupMessage(Guid id, Guid groupId, string senderId, DateTime createdAt, string content)
		{
			Id = id;
			GroupId = groupId;
			SenderId = senderId;
			CreatedAt = createdAt;
			Content = content;
		}
		public static GroupMessage Create(Guid id, Guid groupId, string senderId, DateTime createdAt, string content)
		{
			return new GroupMessage(id, groupId, senderId, createdAt, content);
		}
		public Result SetReference(GroupMessage message)
		{
			if(message.Id == Id)
				return Result.Failure("Message can not be reference to itself");
			ReferenceId = message.Id;
			RefrenceMessage = message;
			return Result.Ok();
		}
		public void AddReactionIfNotExist(User reactor, GroupMessageReaction reaction)
		{
			if (reaction.MesssageId != Id)
				return;

			if (Reactions.Any(x => x.ReactionId == reaction.ReactionId) is false)
				Reactions.Add(reaction);

			GroupMessageReaction getReaction = Reactions.First(x => x.ReactionId == reaction.ReactionId);

			getReaction.AddReactor(reactor.Id);

		}

		
	}
}
