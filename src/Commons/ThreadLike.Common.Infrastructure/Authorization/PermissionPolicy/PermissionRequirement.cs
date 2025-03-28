using Microsoft.AspNetCore.Authorization;

namespace ThreadLike.Common.Infrastructure.Authorization.PermissionPolicy;

internal sealed class PermissionRequirement : IAuthorizationRequirement
{
	public PermissionRequirement(string permission)
	{
		Permission = permission;
	}
	public string Permission { get; }
}
