
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Common.Application.Messaging;
using ThreadLike.Common.Domain;
using ThreadLike.User.Domain.Users;
using ThreadLike.User.Domain.Users.Errors;

namespace ThreadLike.User.Application.Users.Queries.GetById
{
	public record GetUserDetailQuery(string identityId) : IQuery<Result<Domain.Users.User>>;
	internal class GetUserDetailQueryHandler(
		IUserRepository _userRepository
		) : IQueryHandler<GetUserDetailQuery, Result<Domain.Users.User>>
	{

		public async Task<Result<Domain.Users.User>> Handle(GetUserDetailQuery request, CancellationToken cancellationToken)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(request.identityId, nameof(request.identityId));

			Domain.Users.User? getUser = await _userRepository.GetByIdentityIdAsync(request.identityId);
			if (getUser == null)
			{
				return Result.Failure(UserErrors.UserNotFound);
			}
			return getUser;
		}
	}

}
