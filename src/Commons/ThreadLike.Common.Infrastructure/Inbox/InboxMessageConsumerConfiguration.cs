using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ThreadLike.Common.Infrastructure.Inbox;

public sealed class InboxMessageConsumerConfiguration : IEntityTypeConfiguration<InboxMessageConsumer>
{
	public void Configure(EntityTypeBuilder<InboxMessageConsumer> builder)
	{
		//builder.ToTable("inbox_message_consumers");

		builder.HasKey(o => new { o.InboxMessageId, o.Name });

		builder.Property(o => o.Name);//.HasColumnName("name");//.HasMaxLength(500);
		builder.Property(o => o.InboxMessageId);//.HasColumnName("inbox_message_id");
	}
}
