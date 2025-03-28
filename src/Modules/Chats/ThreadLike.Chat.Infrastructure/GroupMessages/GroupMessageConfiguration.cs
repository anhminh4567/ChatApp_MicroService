using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.GroupMessages;
using ThreadLike.Chat.Domain.Groups;
using ThreadLike.Chat.Domain.Users;

namespace ThreadLike.Chat.Infrastructure.GroupMessages
{
	internal class GroupMessageConfiguration : IEntityTypeConfiguration<GroupMessage>
	{
		public void Configure(EntityTypeBuilder<GroupMessage> builder)
		{
			builder.ToTable("GroupMessage");

			builder.HasKey(x => x.Id);

			builder.HasIndex(x => new { x.Id, x.GroupId }).IsUnique();

			builder.HasOne<Group>()
				.WithMany(g => g.Messages )
				.HasForeignKey(g => g.GroupId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne<User>()
				.WithMany(u => u.GroupMessages)
				.HasForeignKey(x => x.SenderId)
				.OnDelete(DeleteBehavior.Restrict);
			
			builder.HasOne<GroupMessage>()
				.WithMany()
				.HasForeignKey(x => x.ReferenceId)
				.IsRequired(false)
				.OnDelete(DeleteBehavior.NoAction);

			builder.Navigation(x => x.RefrenceMessage);
		}
	}
}
