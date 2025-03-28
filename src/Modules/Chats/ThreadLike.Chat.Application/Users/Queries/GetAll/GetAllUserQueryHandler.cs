using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.Users;
using ThreadLike.Common.Application.Messaging;
using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.Application.Users.Queries.GetAll
{
	public record GetAllUserQuery() : IQuery<List<User>>;

	internal class GetAllUserQueryHandler(IUserRepository userRepository) : IQueryHandler<GetAllUserQuery, List<User>>
	{
		public async Task<List<User>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
		{
			var users = await userRepository.GetAll(cancellationToken);
			return users;
		}
	}
}
