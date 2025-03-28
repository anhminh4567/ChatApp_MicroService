using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadLike.Common.Infrastructure.Authorization.RolePolicy
{
	public class RolePolicyAttribute : AuthorizeAttribute
	{
		//private RolePolicyAuthorizeAttribute(string rolesString, RoleRequirementType type) 
		//{
		//	base.Policy = $"{rolesString.Trim()}:{type.ToString()}";
		//}
		public const string Prefix = "CustomRoleHandler";

		public RolePolicyAttribute(string role)
		{
			Policy = $"{Prefix}:{role.Trim()}:{RoleRequirementType.All}";
		}
		public RolePolicyAttribute(string[] roles, RoleRequirementType type = RoleRequirementType.Any)
		{
			string rolesString = string.Join(",", roles);
			Policy = $"{Prefix}:{rolesString}:{type}";
		}
	}
}
