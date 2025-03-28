using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.GroupMessages.Entities;
using ThreadLike.Chat.Domain.Reactions;

namespace ThreadLike.Chat.Infrastructure.GroupMessages
{
	internal class GroupMessageReactionConfiguration : IEntityTypeConfiguration<GroupMessageReaction>
	{
		public void Configure(EntityTypeBuilder<GroupMessageReaction> builder)
		{
			builder.ToTable("GroupMessageReaction");

			builder.HasKey(x => new { x.MesssageId, x.ReactionId });

			builder.HasOne<Reaction>()
				.WithMany()
				.HasForeignKey(x => x.ReactionId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
