﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.GroupMessages;
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

		public Task<User?> GetByIdentityIdAsync(string identityId, CancellationToken cancellationToken = default)
		{
			return _set.FirstOrDefaultAsync(x => x.IdentityId == identityId, cancellationToken);
		}
	}
}
