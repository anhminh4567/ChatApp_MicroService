using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Common.Infrastructure.Authentication;

namespace ThreadLike.Common.Infrastructure.Authorization.RolePolicy
{
	internal class RoleAuthorizationHandler : AuthorizationHandler<RoleRequirement>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
		{
			HashSet<string> roleClaims = context.User.GetRoles();

			string[] roles = requirement.RoleNames.Split(",");

			foreach (string item in roles)
			{
				item.Trim();
			}
			bool isValid = requirement.RequirementType switch
			{
				RoleRequirementType.Any => roles.Any(roleClaims.Contains),
				RoleRequirementType.All => roles.All(roleClaims.Contains),
				_ => throw new NotImplementedException()
			};
			if(isValid)
			{
				context.Succeed(requirement);
			}
			return Task.CompletedTask;
		}
	}
}
