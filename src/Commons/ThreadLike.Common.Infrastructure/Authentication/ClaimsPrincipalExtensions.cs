using System.Security.Claims;

namespace ThreadLike.Common.Infrastructure.Authentication;


// use to modify the incomming accceess token
// since it is from Keycloak, it does not match application businsess
// ==> this parse to business claims
public static class ClaimsPrincipalExtensions
{
	// this some bullshit 
	//	sub -> ClaimTypes.NameIdentifier
	//	name -> ClaimTypes.Name
	//	email -> ClaimTypes.Email
	//	role -> ClaimTypes.Role

	public static string GetIdentityId(this ClaimsPrincipal? principal)
	{
		return principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
			   throw new ApplicationException("User identity is unavailable");
	}
	public static HashSet<string> GetScopes(this ClaimsPrincipal principal)
	{
		Claim scopeClaim = principal.FindFirst(CustomClaims.Scope) ??
						   throw new ApplicationException("Scopes are unavailable");
		return scopeClaim.Value.Split(' ').ToHashSet();
	}
	public static HashSet<string> GetRoles(this ClaimsPrincipal principal)
	{
		IEnumerable<Claim> roleClaim = principal?.FindAll(CustomClaims.Role) ??
						  throw new ApplicationException("Roles are unavailable");

		return roleClaim.Select(c => c.Value).ToHashSet();
	}
	public static string GetEmail(this ClaimsPrincipal principal) 
		=> principal.FindFirst(ClaimTypes.Email)?.Value ??
			throw new ApplicationException("Email is unavailable");
	public static HashSet<string> GetPermissions(this ClaimsPrincipal? principal)
	{
		IEnumerable<Claim> permissionClaims = principal?.FindAll(CustomClaims.Permission) ??
											  throw new ApplicationException("Permissions are unavailable");

		return permissionClaims.Select(c => c.Value).ToHashSet();
	}
}
