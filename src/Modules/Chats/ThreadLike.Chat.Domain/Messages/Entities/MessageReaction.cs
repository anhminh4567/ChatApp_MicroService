using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.Reactions;

namespace ThreadLike.Chat.Domain.Messages.Entities
{
	public class MessageReaction : ReactionObject
	{
		protected MessageReaction(Guid messageId, string reactionId) : base(messageId, reactionId)
		{
		}
		public static MessageReaction Create(Guid messageId, string reactionId)
		{
			return new MessageReaction(messageId, reactionId);
		}

		private MessageReaction() : base(Guid.Empty, string.Empty)
		{
		}
	}
}
