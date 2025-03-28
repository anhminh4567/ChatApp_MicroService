using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.Users;

namespace ThreadLike.Chat.Infrastructure.Users
{
	internal class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.HasKey(u => u.Id);
			builder.Property(u => u.IdentityId).IsRequired().HasMaxLength(100);
			builder.HasIndex(u => u.IdentityId).IsUnique();
			
			builder.HasMany(u => u.Messages)
				.WithOne()
				.HasForeignKey(pm => pm.SenderId)
				.OnDelete(DeleteBehavior.Cascade);


		}
	}
}
