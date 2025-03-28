using Microsoft.EntityFrameworkCore;
using ThreadLike.Common.Application.Caching;
using ThreadLike.Common.Infrastructure.Repositories;
using ThreadLike.Chat.Domain.Groups;
using ThreadLike.Chat.Infrastructure.Database;

namespace ThreadLike.Chat.Infrastructure.Groups
{
    internal class GroupRepository : BaseRepository<Group, ChatDbContext>, IGroupRepository
    {
        private readonly ICacheService _cacheService;

        public GroupRepository(ChatDbContext dbContext, ICacheService cacheService) : base(dbContext)
        {
            _cacheService = cacheService;
        }

    }
}
