using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.Domain.Groups.Entities
{
	public class Participant : Entity
	{
		public string UserId { get; private set; }
		public Guid GroupId { get; private set; }
		public string RoleName { get; private set; }
		public bool IsMuted { get; private set; } = false;
		public DateTime JoinedAt { get; private set; }
		private Participant(string userId, Guid groupId, string roleName)
		{
			UserId = userId;
			GroupId = groupId;
			RoleName = roleName;
			JoinedAt = DateTime.UtcNow;
		}

		public static Participant Create(string userId, Guid groupId, string roleName)
		{
			return new Participant(userId, groupId, roleName);
		}
		public void Mute() => IsMuted = true;
		public void UnMute() => IsMuted = false;

		private Participant()
		{
		}
	}
}
