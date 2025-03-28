using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.Reactions;

namespace ThreadLike.Chat.Domain.GroupMessages.Entities
{
	public class GroupMessageReaction : ReactionObject
	{
		protected GroupMessageReaction(Guid messageId, string reactionId) : base(messageId, reactionId)
		{
		}
		public static GroupMessageReaction Create(Guid messageId, string reactionId)
		{
			return new GroupMessageReaction(messageId, reactionId);
		}

		private GroupMessageReaction() : base(Guid.NewGuid(),"")
		{

		}
	}
}
