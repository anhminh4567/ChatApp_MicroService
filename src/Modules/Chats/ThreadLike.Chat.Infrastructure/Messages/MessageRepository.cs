using Microsoft.EntityFrameworkCore;
using ThreadLike.Common.Application.Caching;
using ThreadLike.Common.Infrastructure.Repositories;
using ThreadLike.Chat.Domain.Messages;
using ThreadLike.Chat.Infrastructure.Database;
using ThreadLike.Chat.Domain.Messages.Entities;

namespace ThreadLike.Chat.Infrastructure.Messages
{
	internal class MessageRepository : BaseRepository<Message, ChatDbContext>, IMessageRepository
    {
        private readonly ICacheService _cacheService;

        public MessageRepository(ChatDbContext dbContext, ICacheService cacheService) : base(dbContext)
        {
            _cacheService = cacheService;
        }

		public Task<List<Message>> GetAllFromGroup(Guid groupId, CancellationToken token = default)
		{
			return _dbContext.Messages.Where(x => x.GroupId == groupId)
				.Include(x => x.MessageAttachments)
				.ToListAsync(token);
		}

		public Task<Message?> GetByIdAndGroup(Guid id, Guid groupId, CancellationToken token = default)
		{
			return _dbContext.Messages
				.Where(x => x.GroupId == groupId && x.Id == id)
				.Include(x => x.MessageAttachments)
				.Include(x => x.MessageReactions)
				.FirstOrDefaultAsync(token);
		}

		public Task<List<Message>> GetFromGroupPaging(Guid groupId, int skip, int take, CancellationToken token = default)
		{
			return _dbContext.Messages.Where(x => x.GroupId == groupId)
				.OrderByDescending(x => x.CreatedAt)
				.Include(x => x.MessageAttachments)
				.Skip(skip)
				.Take(take)
				.ToListAsync(token);
		}

		public Task<List<MessageReaction>> GetMessageReactions(Guid messageId, CancellationToken token = default)
		{
			return _dbContext.MessageReactions
				.Where(x => x.MesssageId == messageId)
				.ToListAsync(token);

		}
	}
}
