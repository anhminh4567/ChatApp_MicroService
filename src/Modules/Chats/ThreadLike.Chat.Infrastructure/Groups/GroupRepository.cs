using Microsoft.EntityFrameworkCore;
using ThreadLike.Common.Application.Caching;
using ThreadLike.Common.Infrastructure.Repositories;
using ThreadLike.Chat.Domain.Groups;
using ThreadLike.Chat.Infrastructure.Database;
using ThreadLike.Chat.Domain.Users;
using Microsoft.EntityFrameworkCore.Internal;
using ThreadLike.Chat.Domain.Groups.Entities;
using System.Linq;

namespace ThreadLike.Chat.Infrastructure.Groups
{
	internal class GroupRepository : BaseRepository<Group, ChatDbContext>, IGroupRepository
    {
        private readonly ICacheService _cacheService;

        public GroupRepository(ChatDbContext dbContext, ICacheService cacheService) : base(dbContext)
        {
            _cacheService = cacheService;
        }

		public override Task<Group?> GetById(params object[] ids)
		{
			return _set.Where(x => x.Id == (Guid)ids[0]).Include(x => x.Participants).FirstOrDefaultAsync();
		}

		public async Task<List<Group>> GetGroupsForUser(User user, CancellationToken token = default)
		{
			List<Group> result = await (from participant in _dbContext.Participants
						 where participant.UserId == user.Id
						 join grp in _dbContext.Groups on participant.GroupId equals grp.Id into joined
						 from p in joined.DefaultIfEmpty()
						 select p).ToListAsync(token);
			return result;
		}

		public Task<List<Participant>> GetParticipantsForGroup(Group group, CancellationToken token = default)
		{
			return _dbContext.Participants
				.Where(x => x.GroupId == group.Id)
				.ToListAsync(token);
		}

		public Task<bool> IsParticipantsCorrect(List<string> participantsId, Group groupToCheckAgainst, CancellationToken token = default)
		{
			return _dbContext.Participants
				.Where(x => x.GroupId == groupToCheckAgainst.Id && participantsId.Contains(x.UserId) )
				.CountAsync(token)
				.ContinueWith(x => x.Result == participantsId.Count);
		}
	}
}
