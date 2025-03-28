using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreadLike.User.Domain.Roles;
using ThreadLike.User.Domain.Users.Entities;

namespace ThreadLike.User.Infrastructure.Roles
{
	internal class RoleConfiguration : IEntityTypeConfiguration<Role>
	{
		public void Configure(EntityTypeBuilder<Role> builder)
		{
			builder.HasKey(x => x.Name);
			builder.HasMany<UserRoles>()
				.WithOne()
				.HasForeignKey(y => y.RoleName);
			builder.HasData(Role.DefaultRoles);
		}
		
	}
}
