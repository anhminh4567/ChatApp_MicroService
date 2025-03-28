using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadLike.Chat.Domain.GroupRoles
{
	public class GroupRole
	{
		public const string GroupLeaderName = "GroupLeader";
		public const string MemberName = "Member";
		public const string GuestName = "Guest";
		public static GroupRole GroupLeader = new GroupRole { Role = GroupLeaderName, Description = "GroupLeader", IsDefault = true, IsSystem = true };
		public static GroupRole Member = new GroupRole { Role = MemberName, Description = "Member", IsDefault = true, IsSystem = true };
		public static GroupRole Guest = new GroupRole { Role = GuestName, Description = "Guest", IsDefault = true, IsSystem = true };

		private GroupRole()
		{
		}

		public string Role { get; set; }
		public string Description { get; set; }
		public bool IsDefault { get; set; }
		public bool IsSystem { get; set; }
	}
}
