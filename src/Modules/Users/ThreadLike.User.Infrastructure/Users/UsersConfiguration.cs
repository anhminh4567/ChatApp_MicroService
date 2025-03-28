using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ThreadLike.User.Infrastructure.Users
{
	internal class UsersConfiguration : IEntityTypeConfiguration<Domain.Users.User>
	{
		public void Configure(EntityTypeBuilder<Domain.Users.User> builder)
		{
			builder.HasKey(x => x.Id);
			builder.HasIndex(x => x.IdentityId).IsUnique();
			builder.HasMany(x => x.Roles)
				.WithOne()
				.HasForeignKey(y => y.UserId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
