using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreadLike.User.Domain.Users.Entities;

namespace ThreadLike.User.Infrastructure.Users
{
	internal class UserRoleConfiguration : IEntityTypeConfiguration<UserRoles>
	{
		public void Configure(EntityTypeBuilder<UserRoles> builder)
		{
			builder.HasKey(b => new {b.UserId, b.RoleName });
			builder.HasIndex(b => new {b.UserId, b.RoleName});
		}
	}
}
