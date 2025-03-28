using Microsoft.EntityFrameworkCore;
using ThreadLike.Common.Application.Caching;
using ThreadLike.Common.Infrastructure.Repositories;
using ThreadLike.Chat.Domain.Reactions;
using ThreadLike.Chat.Infrastructure.Database;

namespace ThreadLike.Chat.Infrastructure.Reactions
{
    internal class ReactionRepository : BaseRepository<Reaction, ChatDbContext>, IReactionRepository
    {
        private readonly ICacheService _cacheService;

        public ReactionRepository(ChatDbContext dbContext, ICacheService cacheService) : base(dbContext)
        {
            _cacheService = cacheService;
        }

        public Task<Reaction?> GetByValueAsync(string value, CancellationToken cancellationToken = default)
        {
            return _dbContext.Reactions.FirstOrDefaultAsync(x => x.Value == value, cancellationToken);
        }
    }
}
