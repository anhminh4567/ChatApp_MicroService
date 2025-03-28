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
	internal class UserFriendConfiguration : IEntityTypeConfiguration<UserFriend>
	{
		public void Configure(EntityTypeBuilder<UserFriend> builder)
		{
			builder.HasKey(uf => new { uf.FriendId, uf.UserId });
			builder.HasOne<User>()
				.WithMany(u => u.Friends)
				.HasForeignKey(uf => uf.UserId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne<User>()
				.WithMany()
				.HasForeignKey(uf => uf.FriendId)
				.OnDelete(DeleteBehavior.Restrict);


		}
	}
}
