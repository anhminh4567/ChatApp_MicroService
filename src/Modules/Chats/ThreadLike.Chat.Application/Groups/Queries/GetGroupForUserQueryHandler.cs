using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.Groups;
using ThreadLike.Chat.Domain.Users;
using ThreadLike.Common.Application.Messaging;
using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.Application.Groups.Queries
{
	public record GetGroupForUserQuery(string IdentityId) : IQuery<Result<List<Group>>>;
	internal class GetGroupForUserQueryHandler(
		IUserRepository userRepository,
		IGroupRepository groupRepository) : IQueryHandler<GetGroupForUserQuery, Result<List<Group>>>
	{
		public async Task<Result<List<Group>>> Handle(GetGroupForUserQuery request, CancellationToken cancellationToken)
		{
			User? user = await userRepository.GetByIdentityIdAsync(request.IdentityId);
			if (user == null)
				return Result.Failure(UserErrors.NotFound);

			List<Group> groups = await groupRepository.GetGroupsForUser(user,cancellationToken);

			return groups;
		}
	}
}
