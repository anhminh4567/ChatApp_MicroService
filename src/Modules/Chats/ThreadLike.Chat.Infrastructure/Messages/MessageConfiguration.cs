using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.Groups;
using ThreadLike.Chat.Domain.Messages;
using ThreadLike.Chat.Domain.Users;

namespace ThreadLike.Chat.Infrastructure.Messages
{
	internal class MessageConfiguration : IEntityTypeConfiguration<Message>
	{
		public void Configure(EntityTypeBuilder<Message> builder)
		{
			builder.ToTable("Message");

			builder.HasKey(x => x.Id);

			builder.HasOne<User>()
				.WithMany(x => x.Messages)
				.HasForeignKey(x => x.SenderId)
				.OnDelete(DeleteBehavior.NoAction);

			builder.HasOne(x => x.RefrenceMessage)
				.WithMany()
				.HasForeignKey(x => x.ReferenceId)
				.IsRequired(false)
				.OnDelete(DeleteBehavior.NoAction);

			builder.HasOne<Group>()
				.WithMany(x => x.Messages)
				.HasForeignKey(x => x.GroupId)
				.OnDelete(DeleteBehavior.Cascade);


			builder.HasMany(x => x.MessageReactions)
				.WithOne()
				.HasForeignKey(x => x.MesssageId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.MessageAttachments)
				.WithOne()
				.HasForeignKey(x => x.MessageId)
				.OnDelete(DeleteBehavior.Cascade);

			

		}
	}
}
