using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.Groups;

namespace ThreadLike.Chat.Infrastructure.Groups
{
	internal class GroupConfiguration : IEntityTypeConfiguration<Group>
	{
		public void Configure(EntityTypeBuilder<Group> builder)
		{
			builder.HasKey(x => x.Id);

			builder.HasMany(x => x.Participants)
				.WithOne()
				.HasForeignKey(x => x.GroupId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.Messages)
				.WithOne()
				.HasForeignKey(gm => gm.GroupId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.Creator)
				.WithMany()
				.HasForeignKey(x => x.CreatorId)
				.IsRequired()
				.OnDelete(DeleteBehavior.NoAction);

			builder.OwnsOne(x => x.ThumbDetail, build => build.ToJson());

		}
	}
}
