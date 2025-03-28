using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Infrastructure.Database;
using ThreadLike.Common.Application.Authorization;
using ThreadLike.Common.Application.Authorization.Response;
using ThreadLike.Common.Application.Caching;
using ThreadLike.Common.Contracts;
using ThreadLike.Common.Domain;


namespace ThreadLike.Chat.Infrastructure.Authorization
{
	internal class AuthorizationService(
		ChatDbContext _dbContext,
		ICacheService _cacheService,
		IRequestClient<UsersModuleContracts.GetUserRolesRequest> _client) : IAuthorizationService
	{

		public async Task<Result<RoleResponse>> GetResultRolesAsync(string identityId)
		{
			string cacheKey = ((IAuthorizationService)this).GetCacheKey(IAuthorizationService.RolePrefix, identityId);

			RoleResponse? cachedRoles = await _cacheService.GetAsync<RoleResponse>(cacheKey);

			if (cachedRoles != null)
			{
				return cachedRoles;
			}

			Response<UsersModuleContracts.GetUserRolesResponse> response = await _client.GetResponse<UsersModuleContracts.GetUserRolesResponse>(new UsersModuleContracts.GetUserRolesRequest(identityId));

			if (response.Message is null)
				return Result.Failure("no role found");

			if(response.Message.Roles.Count == 0 || response.Message.userId is null)
				return Result.Failure("no role found for this user");

			string? userId = response.Message.userId;
			
			var roleResponse =  new RoleResponse(userId,response.Message.Roles.ToHashSet());
			
			await _cacheService.SetAsync(cacheKey, roleResponse);

			return roleResponse;
		}

		public Task<Result<PermissionsResponse>> GetUserPermissionsAsync(string identityId)
		{
			throw new NotImplementedException();
		}
	}
}
