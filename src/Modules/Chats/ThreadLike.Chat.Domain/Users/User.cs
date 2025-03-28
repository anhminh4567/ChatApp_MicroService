using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.Messages;
using ThreadLike.Chat.Domain.Users.Entities;
using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.Domain.Users
{
	public class User : Entity
	{
		public string Id { get; private set; }
		public string Name { get; private set; }
		public string Email { get; private set; }
		public string IdentityId { get; private set; }
		public string? AvatarUri { get; private set; }
		public DateTime CreatedAt { get; private set; }
		public DateTime UpdatedAt { get; private set; }
		public bool IsVerified { get; private set; }
		public List<Message> Messages { get; private set; } = new();
		public List<UserFriend> Friends { get; private set; } = new();
		public List<UserLetter> Letters { get; private set; } = new();
		protected User(string id, string name, string email, string identityId, string? avatarUri, DateTime createdAt, DateTime updatedAt)
		{
			Id = id;
			Name = name;
			Email = email;
			IdentityId = identityId;
			AvatarUri = avatarUri;
			CreatedAt = createdAt;
			UpdatedAt = updatedAt;
			IsVerified = false;
		}
		public static User Create(string name, string email, string identityId, string? avatarUri = null, DateTime? CreatedAt =null)
		{
			var dateTime = DateTime.UtcNow;
			var user = new User(Guid.NewGuid().ToString(), name, email, identityId, avatarUri, dateTime, CreatedAt ?? dateTime);
			// Raise domain event if necessary
			return user;
		}
		public void ChangeAvatarUri(string avatarUri)
		{
			AvatarUri = avatarUri;
			UpdatedAt = DateTime.UtcNow;
			// Raise domain event if necessary
		}
		public void AddFriend(User user)
		{
			if (user.Id == Id)
			{
				return;
			}
			Friends.Add(new UserFriend(Id,user.Id));
		}
		public bool Verify() => IsVerified = true;
	}
}
