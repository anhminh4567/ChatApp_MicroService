using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Common.Domain;
using ThreadLike.User.Domain.Roles;
using ThreadLike.User.Domain.Users.Entities;

namespace ThreadLike.User.Domain.Users
{
	public interface IUserRepository : IBaseRepository<User>
	{
		Task<User?> GetByIdentityIdAsync(string identityId, CancellationToken cancellationToken = default);
		Task<List<UserRoles>> GetUserRoles(string identityId, CancellationToken cancellationToken = default);


	}
}
