using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.Messages.Entities;
using ThreadLike.Chat.Domain.Reactions;

namespace ThreadLike.Chat.Infrastructure.Messages
{
	internal class MessageReactionConfiguration : IEntityTypeConfiguration<MessageReaction>
	{
		public void Configure(EntityTypeBuilder<MessageReaction> builder)
		{
			builder.ToTable("MessageReaction");

			builder.HasKey(x => new { x.MesssageId, x.ReactionId });
			
			builder.HasOne<Reaction>()
				.WithMany()
				.HasForeignKey(x => x.ReactionId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
