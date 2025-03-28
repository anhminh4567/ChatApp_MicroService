using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.Users;
using ThreadLike.Common.Application.Messaging;
using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.Application.Users.Queries.GetDetail
{
	public record GetUserDetailQuery(string Id) : IQuery<Result<User>>;
	internal class GetUserDetailQueryHandler(
		IUserRepository userRepository)
		: IQueryHandler<GetUserDetailQuery, Result<User>>
	{
		public async Task<Result<User>> Handle(GetUserDetailQuery request, CancellationToken cancellationToken)
		{
			User user = await userRepository.GetById(request.Id);

			if (user == null)
			{
				return Result.Failure(Error.NotFound("UserNotFound", $"User with ID {request.Id} was not found."));
			}

			return user;
		}
	}
}
