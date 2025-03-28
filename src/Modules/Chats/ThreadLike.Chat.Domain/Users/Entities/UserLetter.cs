using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.Users.Enums;

namespace ThreadLike.Chat.Domain.Users.Entities
{
	public class UserLetter
	{

		public Guid Id { get; private set; }
		public string? SenderId { get; private set; }
		public string ReceiverId { get; private set; }
		public UserLetterType LetterType { get; private set; }
		public string Content { get; private set; }
		public DateTime SentAt { get; private set; } = DateTime.UtcNow;
		public DateTime? ProcessedAt { get;private set; } 
		public bool IsHtml { get; private set; }
		public DateTime ExpireTime { get; private set; } 
		public bool IsRead { get; private set; } = false;
		private UserLetter(Guid id, string? senderId, string receiverId, UserLetterType letterType, string content, bool isHtml, DateTime expireTime)
		{
			Id = id;
			SenderId = senderId;
			ReceiverId = receiverId;
			LetterType = letterType;
			Content = content;
			IsHtml = isHtml;
			ExpireTime = expireTime;
		}

		public static UserLetter CreateFriendRequest(User sender, User receiver, string content = "{sendername} want to send you friend request ?")
		{
			string formattedContent = string.Format(content, sender.Name);
			return new UserLetter(Guid.NewGuid(), sender.Id, receiver.Id, UserLetterType.FriendRequest, formattedContent, false, DateTime.UtcNow.AddDays(1));
		}
		public void Read() => IsRead = true;
	}
}
