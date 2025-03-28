using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadLike.User.Domain.Roles
{
	public class Role 
	{
		public static List<Role> DefaultRoles = new()
		{
			new Role { Name = AdminRoleName , Description = "Admin" },
			new Role { Name = UserRoleName , Description = "User" },
			new Role { Name = GuestRoleName , Description = "Guest" },
			new Role { Name = ModeratorRoleName , Description = "Moderator" }
		};
		public static Role Admin => DefaultRoles[0];
		public static Role User => DefaultRoles[1];
		public static Role Guest => DefaultRoles[2];
		public static Role Moderator => DefaultRoles[3];
		public const string AdminRoleName = "Admin";
		public const string UserRoleName = "User";
		public const string GuestRoleName = "Guest";
		public const string ModeratorRoleName = "Moderator";
		public string Name { get;  set; }
		public string? Description { get;  set; }
		
	}
}
