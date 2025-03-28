using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.Messages.Entities;
using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.Domain.Reactions
{
	public class ReactionObject 
	{
		public Guid MesssageId { get; private set; }
		public string ReactionId { get; private set; }
		public int Count => ReactionId.Length;
		public List<string> ReactorIds { get; private set; } = [];
		public ReactionObject(Guid messageId, string reactionId)
		{
			MesssageId = messageId;
			ReactionId = reactionId;
		}
		public static ReactionObject Create(Guid messageId, string reactionId)
		{
			return new ReactionObject(messageId, reactionId);
		}
		public void AddReactor(string userId)
		{
			ReactorIds.Add(userId);
		}
		public void RemoveReactor(string userId)
		{
			ReactorIds.Remove(userId);
		}
	}
}
