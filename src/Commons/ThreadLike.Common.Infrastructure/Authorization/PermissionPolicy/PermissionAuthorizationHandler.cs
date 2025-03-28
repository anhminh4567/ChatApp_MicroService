using Microsoft.AspNetCore.Authorization;
using ThreadLike.Common.Infrastructure.Authentication;

namespace ThreadLike.Common.Infrastructure.Authorization.PermissionPolicy;

internal sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
	protected override Task HandleRequirementAsync(
		AuthorizationHandlerContext context,
		PermissionRequirement requirement)
	{
		HashSet<string> permissions = context.User.GetPermissions();

		if (permissions.Contains(requirement.Permission))
		{
			context.Succeed(requirement);
		}

		return Task.CompletedTask;
	}
}
