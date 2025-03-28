using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.Messages.Entities;
using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.Domain.Messages
{
	public class Message : Entity
	{
		public const string MESSAGER_MARK_DELETED = "%%MESSAGE_MARK_DELETED%%";
		public Guid Id { get; private set; }
		public string SenderId { get; private set; }
		public string ReceiverId { get; private set; }
		public string Content { get; private set; } = "";
		public Guid? ReferenceId { get; private set; }
		public Message? RefrenceMessage { get; private set; }
		public List<MessageReaction> MessageReactions { get; private set; } = new();
		public DateTime CreatedAt { get; private set; }
		public DateTime? DeletedAt { get; private set; }

		private Message(Guid id, string senderId, string receiverId, string content, Guid? referenceId, Message? referenceMessage, DateTime createdAt)
		{
			Id = id;
			SenderId = senderId;
			ReceiverId = receiverId;
			Content = content;
			ReferenceId = referenceId;
			RefrenceMessage = referenceMessage;
			CreatedAt = createdAt;
		}

		public static Message Create(Guid id, string senderId, string receiverId, string content, Guid? referenceId, Message? referenceMessage, DateTime createdAt)
		{
			return new Message(id, senderId, receiverId, content, referenceId, referenceMessage, createdAt);
		}
		public void Delete()
		{
			DeletedAt = DateTime.UtcNow;
		}

		private Message()
		{

		}
	}
}
