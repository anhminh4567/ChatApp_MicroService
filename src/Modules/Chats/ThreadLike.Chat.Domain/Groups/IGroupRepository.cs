using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.Groups.Entities;
using ThreadLike.Chat.Domain.Users;
using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.Domain.Groups
{
	public interface IGroupRepository : IBaseRepository<Group>
	{
		Task<List<Group>> GetGroupsForUser(User user,CancellationToken token = default);
		Task<List<Participant>> GetParticipantsForGroup(Group group, CancellationToken token = default);
		Task<bool> IsParticipantsCorrect(List<string> participantsId, Group groupToCheckAgainst, CancellationToken token = default);
	}
}
