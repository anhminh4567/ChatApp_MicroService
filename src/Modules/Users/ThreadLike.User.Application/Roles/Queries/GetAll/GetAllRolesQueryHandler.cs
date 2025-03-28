using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Common.Application.Messaging;
using ThreadLike.Common.Domain;
using ThreadLike.User.Domain.Roles;

namespace ThreadLike.User.Application.Roles.Queries.GetAll
{
	public record GetAllRolesQuery() : IQuery<List<Role>>;
	internal class GetAllRolesQueryHandler(IRoleRepository _roleRepository) : IQueryHandler<GetAllRolesQuery, List<Role>>
	{	
		public Task<List<Role>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
		{
			return _roleRepository.GetAll(cancellationToken);
		}
	}

}
