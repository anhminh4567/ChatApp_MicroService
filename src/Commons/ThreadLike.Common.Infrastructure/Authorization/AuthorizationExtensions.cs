using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using ThreadLike.Common.Infrastructure.Authorization.PermissionPolicy;
using ThreadLike.Common.Infrastructure.Authorization.RolePolicy;

namespace ThreadLike.Common.Infrastructure.Authorization;

internal static class AuthorizationExtensions
{
	internal static IServiceCollection AddAuthorizationInternal(this IServiceCollection services)
	{
		//services.AddTransient<IClaimsTransformation, CustomClaimsTransformation>();

		services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

		//services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

		services.AddScoped<IAuthorizationHandler, RoleAuthorizationHandler>();

		services.AddSingleton<IAuthorizationPolicyProvider, CustomAuthorizationPolicyProvider>();
		
		services.AddTransient<IClaimsTransformation, RoleClaimsTransformation>();

		services.AddAuthorization();
		return services;
	}
}
