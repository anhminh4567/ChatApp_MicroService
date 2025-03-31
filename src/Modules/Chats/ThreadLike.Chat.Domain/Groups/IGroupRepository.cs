using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.Users;
using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.Domain.Groups
{
	public interface IGroupRepository : IBaseRepository<Group>
	{
		Task<List<Group>> GetGroupsForUser(User user,CancellationToken token = default);
	}
}
