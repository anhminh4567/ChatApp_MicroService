using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.Users;
using ThreadLike.Chat.Domain.Users.Entities;

namespace ThreadLike.Chat.Infrastructure.Users
{
	internal class UserLetterConfiguration : IEntityTypeConfiguration<UserLetter>
	{
		public void Configure(EntityTypeBuilder<UserLetter> builder)
		{
			builder.HasKey(ul => ul.Id);
			builder.HasOne<User>()
				.WithMany(u => u.Letters)
				.HasForeignKey(ul => ul.ReceiverId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne<User>()
				.WithMany()
				.HasForeignKey(ul => ul.SenderId)
				.IsRequired(false)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
