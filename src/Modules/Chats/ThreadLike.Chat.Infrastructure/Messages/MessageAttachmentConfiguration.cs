using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.Messages.Entities;

namespace ThreadLike.Chat.Infrastructure.Messages
{
	internal class MessageAttachmentConfiguration : IEntityTypeConfiguration<MessageAttachment>
	{
		public void Configure(EntityTypeBuilder<MessageAttachment> builder)
		{
			builder.HasKey(x => new {x.Id, x.MessageId});

			builder.OwnsOne(x => x.ThumbDetail, build => build.ToJson());
			builder.OwnsOne(x => x.AttachmentDetail, build => build.ToJson());


		}
	}
}
