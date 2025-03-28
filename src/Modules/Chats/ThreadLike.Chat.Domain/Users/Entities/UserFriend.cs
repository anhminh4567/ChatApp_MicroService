using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadLike.Chat.Domain.Users.Entities
{
	public class UserFriend
	{
		public string UserId { get; set; }
		public string FriendId { get; set; }
		public UserFriend(string userId, string friendId)
		{
			UserId = userId;
			FriendId = friendId;
		}
	}
}
