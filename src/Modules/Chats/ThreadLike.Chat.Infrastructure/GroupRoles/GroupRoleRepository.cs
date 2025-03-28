using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.GroupMessages;
using ThreadLike.Chat.Domain.GroupRoles;
using ThreadLike.Chat.Infrastructure.Database;
using ThreadLike.Common.Application.Caching;
using ThreadLike.Common.Infrastructure.Repositories;

namespace ThreadLike.Chat.Infrastructure.GroupRoles
{
	internal class GroupRoleRepository : BaseRepository<GroupRole, ChatDbContext>, IGroupRoleRepository
	{
		private readonly ICacheService _cacheService;

		public GroupRoleRepository(ChatDbContext dbContext, ICacheService cacheService) : base(dbContext)
		{
			_cacheService = cacheService;
		}
	}
}
