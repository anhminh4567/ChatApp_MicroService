using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Common.Application.Exceptions;
using ThreadLike.Common.Infrastructure.Authorization.RolePolicy;

namespace ThreadLike.Common.Infrastructure.Authorization
{
	internal class CustomAuthorizationPolicyProvider : IAuthorizationPolicyProvider
	{
		public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }
		private readonly AuthorizationOptions _authorizationOptions;

		public CustomAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
		{
			_authorizationOptions = options.Value;
			FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
		}

		public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();

		public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => FallbackPolicyProvider.GetFallbackPolicyAsync();
		public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
		{
			AuthorizationPolicy? policy = _authorizationOptions.GetPolicy(policyName);
			if(policy != null)
			{
				return policy;
			}

			if( !policyName.StartsWith(RolePolicyAttribute.Prefix) )
			{
				return await FallbackPolicyProvider.GetPolicyAsync(policyName);
			}

			string[] splitedPolicyValues = policyName.Split(":");
			if(splitedPolicyValues.Count() != 3)
				throw new TheadlikeApplicationException("AuthorizationPolicyError_Major");


			AuthorizationPolicy newPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
				.AddRequirements(new RoleRequirement(splitedPolicyValues[1], Enum.Parse<RoleRequirementType>(splitedPolicyValues[2])))
				.Build();

			_authorizationOptions.AddPolicy(policyName, newPolicy);
			return newPolicy;
		}
	}
}
