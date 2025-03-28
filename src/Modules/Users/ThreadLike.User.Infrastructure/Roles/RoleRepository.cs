using Microsoft.EntityFrameworkCore;
using ThreadLike.Common.Infrastructure.Repositories;
using ThreadLike.User.Domain.Roles;
using ThreadLike.User.Domain.Users.Entities;
using ThreadLike.User.Infrastructure.Database;

namespace ThreadLike.User.Infrastructure.Roles
{
	internal class RoleRepository : BaseRepository<Role, UserDbContext>, IRoleRepository
	{
		public RoleRepository(UserDbContext dbContext) : base(dbContext)
		{
		}

		
	}
}
