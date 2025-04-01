using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Common.Application.Authentication;
using ThreadLike.Common.Application.Authorization;
using ThreadLike.Common.Application.Authorization.Response;
using ThreadLike.Common.Application.Exceptions;
using ThreadLike.Common.Domain;

namespace ThreadLike.Common.Infrastructure.Authorization.RolePolicy
{
	internal class RoleClaimsTransformation : IClaimsTransformation
	{
		private readonly IServiceScopeFactory _serviceScopeFactory;

		public RoleClaimsTransformation(IServiceScopeFactory serviceScopeFactory)
		{
			_serviceScopeFactory = serviceScopeFactory;
		}

		public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
		{
			//if (principal.HasClaim(c => c.Type == CustomClaims.Sub))
			//	return principal;

			using IServiceScope scope = _serviceScopeFactory.CreateScope();
			// this is our service, not from microsoft

			var authorizationService = scope.ServiceProvider.GetRequiredService<IAuthorizationService>();
			
			string? identityId = principal.GetIdentityId(); // this is an extension method ( no need to care )

			if (identityId == null)
				throw new TheadlikeApplicationException("User identity is unavailable");

			Result<RoleResponse> result = await authorizationService.GetResultRolesAsync(identityId);
			
			if (result.IsFailure)
				throw new TheadlikeApplicationException(nameof(IAuthorizationService.GetResultRolesAsync), result.Error);

			var claimsIdentity = new ClaimsIdentity();
			
			claimsIdentity.AddClaim(new Claim(CustomClaims.UserId, result.Value.UserId));

			foreach (string role in result.Value.Roles)
			{
				claimsIdentity.AddClaim(new Claim(CustomClaims.Role, role));
			}

			principal.AddIdentity(claimsIdentity);
			return principal;
		}
	}
}
