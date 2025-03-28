using Microsoft.EntityFrameworkCore;
using ThreadLike.Common.Application.Caching;
using ThreadLike.Common.Infrastructure.Repositories;
using ThreadLike.Chat.Domain.Messages;
using ThreadLike.Chat.Infrastructure.Database;

namespace ThreadLike.Chat.Infrastructure.Messages
{
    internal class MessageRepository : BaseRepository<Message, ChatDbContext>, IMessageRepository
    {
        private readonly ICacheService _cacheService;

        public MessageRepository(ChatDbContext dbContext, ICacheService cacheService) : base(dbContext)
        {
            _cacheService = cacheService;
        }


    }
}
