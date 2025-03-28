using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using ThreadLike.Common.Application.Authorization;
using ThreadLike.Common.Application.Authorization.Response;
using ThreadLike.Common.Application.Exceptions;
using ThreadLike.Common.Domain;
using ThreadLike.Common.Infrastructure.Authentication;

namespace ThreadLike.Common.Infrastructure.Authorization.PermissionPolicy;

internal sealed class PermissionClaimsTransformation : IClaimsTransformation
{
	private readonly IServiceScopeFactory _serviceScopeFactory;

	public PermissionClaimsTransformation(IServiceScopeFactory serviceScopeFactory)
	{
		_serviceScopeFactory = serviceScopeFactory;
	}

	public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
	{
		if (principal.HasClaim(c => c.Type == CustomClaims.Sub))
			return principal;

		using IServiceScope scope = _serviceScopeFactory.CreateScope();
		// this is our service, not from microsoft
		IAuthorizationService permissionService = scope.ServiceProvider.GetRequiredService<IAuthorizationService>();
		string identityId = principal.GetIdentityId(); // this is an extension method ( no need to care )

		Result<PermissionsResponse> result = await permissionService.GetUserPermissionsAsync(identityId);
		if (result.IsFailure)
			throw new TheadlikeApplicationException(nameof(IAuthorizationService.GetUserPermissionsAsync), result.Error);

		var claimsIdentity = new ClaimsIdentity();
		claimsIdentity.AddClaim(new Claim(CustomClaims.Sub, result.Value.UserId.ToString()));

		foreach (string permission in result.Value.Permissions)
		{
			claimsIdentity.AddClaim(new Claim(CustomClaims.Permission, permission));
		}

		principal.AddIdentity(claimsIdentity);
		return principal;
	}
}
