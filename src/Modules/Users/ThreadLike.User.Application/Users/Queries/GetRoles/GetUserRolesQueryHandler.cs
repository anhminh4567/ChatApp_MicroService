using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Common.Application.Messaging;
using ThreadLike.Common.Domain;
using ThreadLike.User.Domain.Roles;
using ThreadLike.User.Domain.Users;
using ThreadLike.User.Domain.Users.Entities;

namespace ThreadLike.User.Application.Users.Queries.GetRoles
{
	public record GetUserRolesQuery(string IdentityId) : IQuery<List<UserRoles>>;
	internal class GetUserRolesQueryHandler(IUserRepository _userRepository) : IQueryHandler<GetUserRolesQuery, List<UserRoles>>
	{
		
		public async Task<List<UserRoles>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
		{
			return await _userRepository.GetUserRoles(request.IdentityId, cancellationToken);
		}
	}
}
