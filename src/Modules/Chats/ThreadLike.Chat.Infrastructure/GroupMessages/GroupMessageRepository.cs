using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.GroupMessages;
using ThreadLike.Chat.Infrastructure.Database;
using ThreadLike.Common.Application.Caching;
using ThreadLike.Common.Infrastructure.Repositories;

namespace ThreadLike.Chat.Infrastructure.GroupMessages
{
	internal class GroupMessageRepository : BaseRepository<GroupMessage, ChatDbContext>, IGroupMessageRepository
	{
		private readonly ICacheService _cacheService;

		public GroupMessageRepository(ChatDbContext dbContext, ICacheService cacheService) : base(dbContext)
		{
			_cacheService = cacheService;
		}

	}
}
