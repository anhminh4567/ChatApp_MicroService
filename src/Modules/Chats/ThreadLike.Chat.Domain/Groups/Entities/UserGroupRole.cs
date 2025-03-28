using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.Domain.Group.Entities
{
	public class UserGroupRole : Entity
	{
		public string UserId { get; private set; }
		public Guid GroupId { get; private set; }
		public string RoleName{ get; private set; }


		private UserGroupRole(string userId, Guid groupId, string roleName)
		{
			UserId = userId;
			GroupId = groupId;
			RoleName = roleName;
		}

		public static UserGroupRole Create(string userId, Guid groupId, string roleName)
		{
			return new UserGroupRole(userId, groupId, roleName);
		}

		private UserGroupRole()
		{
		}
	}
}
