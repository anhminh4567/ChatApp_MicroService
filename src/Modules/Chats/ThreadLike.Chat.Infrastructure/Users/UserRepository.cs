using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.Users;
using ThreadLike.Chat.Infrastructure.Database;
using ThreadLike.Common.Application.Caching;
using ThreadLike.Common.Infrastructure.Repositories;

namespace ThreadLike.Chat.Infrastructure.Users
{
	internal class UserRepository : BaseRepository<User, ChatDbContext>, IUserRepository
	{
		private readonly ICacheService _cacheService;

		public UserRepository(ChatDbContext dbContext, ICacheService cacheService) : base(dbContext)
		{
			_cacheService = cacheService;
		}

		public override Task<User?> GetById(params object[] ids)
		{
			return _set.Where(x => x.Id == (string)ids[0])
				.Include(x => x.Friends)
				.Include(x => x.Letters)
				.FirstOrDefaultAsync();
		}

		public async Task<bool> Exist(List<string> userIds, CancellationToken cancellationToken = default)
		{
			int count = await _set.Where(x => userIds.Contains(x.Id))
				.CountAsync(cancellationToken);
			return count == userIds.Count;
		}

		public Task<User?> GetByIdentityIdAsync(string identityId, CancellationToken cancellationToken = default)
		{
			return _set.FirstOrDefaultAsync(x => x.IdentityId == identityId, cancellationToken);
		}

		public Task<List<User>> GetByManyIds(List<string> userIds, CancellationToken cancellationToken = default)
		{
			return _set.Where(x => userIds.Contains(x.Id)).ToListAsync(cancellationToken);
		}
	}
}
