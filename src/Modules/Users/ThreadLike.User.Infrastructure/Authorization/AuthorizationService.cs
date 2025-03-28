using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Common.Application.Authorization;
using ThreadLike.Common.Application.Authorization.Response;
using ThreadLike.Common.Application.Caching;
using ThreadLike.Common.Domain;
using ThreadLike.User.Application.Users.Queries.GetRoles;
using ThreadLike.User.Domain.Users.Entities;
using ThreadLike.User.Domain.Users.Errors;
using ThreadLike.User.Infrastructure.Database;

namespace ThreadLike.User.Infrastructure.Authorization
{
	internal class AuthorizationService(
		UserDbContext _dbContext,
		ICacheService _cacheService,
		IMediator _mediator) : IAuthorizationService
	{

		public async Task<Result<RoleResponse>> GetResultRolesAsync(string identityId)
		{
			string cacheKey = ((IAuthorizationService)this).GetCacheKey(IAuthorizationService.RolePrefix, identityId);

			RoleResponse? cachedRoles = await _cacheService.GetAsync<RoleResponse>(cacheKey);

			if (cachedRoles != null)
			{
				return cachedRoles;
			}
			List<UserRoles> result = await _mediator.Send(new GetUserRolesQuery(identityId));

			string? userId = result.FirstOrDefault()?.UserId;
			
			if(userId is null)
				return Result.Failure(UserErrors.UserExist(identityId));
			
			var roleResponse =  new RoleResponse(userId,result.Select(x => x.RoleName).ToHashSet());
			
			await _cacheService.SetAsync(cacheKey, roleResponse);

			return roleResponse;
		}

		public Task<Result<PermissionsResponse>> GetUserPermissionsAsync(string identityId)
		{
			throw new NotImplementedException();
		}
		//private string GetCacheKey(string prefix,string identityId) => $"{prefix}:{identityId}";
	}
}
