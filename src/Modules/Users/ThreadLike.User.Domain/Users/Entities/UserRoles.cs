using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.User.Domain.Roles;

namespace ThreadLike.User.Domain.Users.Entities
{
	public class UserRoles
	{
		public string UserId { get; set; } = default!;
		public string RoleName { get; set; } = default!;
		protected UserRoles()
		{

		}
		public UserRoles(string userId, string roleName)
		{
			ArgumentNullException.ThrowIfNull(userId, nameof(userId));
			ArgumentNullException.ThrowIfNull(roleName, nameof(roleName));
			UserId = userId;
			RoleName = roleName;
		}
		public static UserRoles Create(User user, Role role)
		{
			return new UserRoles
			{
				UserId = user.Id,
				RoleName = role.Name
			};
		}
	}
}
