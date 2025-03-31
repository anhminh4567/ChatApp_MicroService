using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.GroupRoles;
using ThreadLike.Common.Application.Messaging;
using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.Application.GroupRoles.Queries
{
	public record GetAllGroupRolesQuery : IQuery<Result<List<GroupRole>>>;

	internal class GetAllGroupRolesQueryHandler(
		IGroupRoleRepository groupRoleRepository)
		: IQueryHandler<GetAllGroupRolesQuery, Result<List<GroupRole>>>
	{
		public async Task<Result<List<GroupRole>>> Handle(GetAllGroupRolesQuery request, CancellationToken cancellationToken)
		{
			List<GroupRole> groupRoles = await groupRoleRepository.GetAll(cancellationToken);
			return Result.Ok(groupRoles);
		}
	}
}
