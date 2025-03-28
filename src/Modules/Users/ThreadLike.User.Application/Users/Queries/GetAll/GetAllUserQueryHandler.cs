using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Common.Application.Messaging;
using ThreadLike.Common.Domain;
using ThreadLike.User.Domain.Users;

namespace ThreadLike.User.Application.Users.Queries.GetAll
{
	public record GetAllUserQuery(): IQuery<List<Domain.Users.User>>;
	internal class GetAllUserQueryHandler(
		IUserRepository _userRepository
		) : IQueryHandler<GetAllUserQuery, List<Domain.Users.User>>
	{
		
		public async Task<List<Domain.Users.User>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
		{
			List<Domain.Users.User> users = await _userRepository.GetAll(cancellationToken);
			return users;
		}
	}
}
