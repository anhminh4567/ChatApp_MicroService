using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.Groups;
using ThreadLike.Chat.Domain.Messages.Entities;
using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.Domain.Messages
{
	public class Message : Entity
	{
		public const string MESSAGER_MARK_DELETED = "%%MESSAGE_MARK_DELETED%%";
		public Guid Id { get; private set; }
		public string SenderId { get; private set; }
		public Guid GroupId { get; private set; }
		public string Content { get; private set; } = "";
		public Guid? ReferenceId { get; private set; }
		public Message? RefrenceMessage { get; private set; }
		public List<MessageReaction> MessageReactions { get; private set; } = new();
		public List<MessageAttachment> MessageAttachments { get; private set; } = new();
		public DateTime CreatedAt { get; private set; }
		public DateTime? DeletedAt { get; private set; }

		private Message(Guid id, string senderId, Guid groupId, string content, Guid? referenceId, DateTime createdAt)
		{
			Id = id;
			SenderId = senderId;
			Content = content;
			ReferenceId = referenceId;
			CreatedAt = createdAt;
			GroupId = groupId;
		}

		public static Message Create( string senderId, Group group, string content, Message? referenceMessage = null)
		{
			return new Message(Guid.NewGuid(), senderId, group.Id, content, referenceMessage?.Id, DateTime.UtcNow);
		}
		public void Delete()
		{
			DeletedAt = DateTime.UtcNow;
		}
		public void SetAttachment(MessageAttachment attachment)
		{
			MessageAttachments.Add(attachment);
		}
		private Message()
		{

		}
	}
}
