using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.Domain.Groups.Entities
{
	public class Participants : Entity
	{
		public string UserId { get; private set; }
		public Guid GroupId { get; private set; }
		public string RoleName { get; private set; }
		public bool IsMuted { get; private set; } = false;
		public DateTime JoinedAt { get; private set; }
		private Participants(string userId, Guid groupId, string roleName)
		{
			UserId = userId;
			GroupId = groupId;
			RoleName = roleName;
			JoinedAt = DateTime.Now;
		}

		public static Participants Create(string userId, Guid groupId, string roleName)
		{
			return new Participants(userId, groupId, roleName);
		}
		public void Mute() => IsMuted = true;
		public void UnMute() => IsMuted = false;

		private Participants()
		{
		}
	}
}
