using Microsoft.EntityFrameworkCore;
using ThreadLike.Common.Application.Caching;
using ThreadLike.Common.Infrastructure.Repositories;
using ThreadLike.User.Domain.Users;
using ThreadLike.User.Domain.Users.Entities;
using ThreadLike.User.Domain.Users.Errors;
using ThreadLike.User.Infrastructure.Database;

namespace ThreadLike.User.Infrastructure.Users
{
	internal class UserRepository : BaseRepository<Domain.Users.User, UserDbContext>, IUserRepository
	{
		private readonly ICacheService _cacheService;
		public UserRepository(UserDbContext dbContext, ICacheService cacheService) : base(dbContext)
		{
			_cacheService = cacheService;
		}

		public Task<Domain.Users.User?> GetByIdentityIdAsync(string identityId, CancellationToken cancellationToken = default)
		{
			return _dbContext.Users.FirstOrDefaultAsync(x => x.IdentityId == identityId, cancellationToken);
		}
		public async Task<List<UserRoles>> GetUserRoles(string identityId, CancellationToken cancellationToken = default)
		{
			List<UserRoles> userRoles = await _dbContext.Users
			   .Where(x => x.IdentityId == identityId)
			   .Join(_dbContext.UserRoles,
					 user => user.Id,
					 userRole => userRole.UserId,
					 (user, userRole) => userRole)
			   .ToListAsync();
			return userRoles is null ? [] :  userRoles;
		}
	}
}
