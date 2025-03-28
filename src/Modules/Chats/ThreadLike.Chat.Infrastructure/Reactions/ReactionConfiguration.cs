using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.Reactions;

namespace ThreadLike.Chat.Infrastructure.Reactions
{
	internal class ReactionConfiguration : IEntityTypeConfiguration<Reaction>
	{
		public void Configure(EntityTypeBuilder<Reaction> builder)
		{
			builder.HasKey(x => x.Id);

		}
	}
}
