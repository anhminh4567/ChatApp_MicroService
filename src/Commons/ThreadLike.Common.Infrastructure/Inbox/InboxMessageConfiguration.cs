using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ThreadLike.Common.Infrastructure.Inbox;

public sealed class InboxMessageConfiguration : IEntityTypeConfiguration<InboxMessage>
{
	public void Configure(EntityTypeBuilder<InboxMessage> builder)
	{
		//builder.ToTable("inbox_messages");

		builder.HasKey(o => o.Id);
		builder.Property(o => o.Id);//.HasColumnName("id");
		builder.Property(o => o.Content).HasColumnType("jsonb");//.HasColumnName("content").HasColumnType("jsonb");//.HasMaxLength(2000).HasColumnType("jsonb");
		builder.Property(o => o.Type);//.HasColumnName("type");
		builder.Property(o => o.ProcessedOnUtc);//.HasColumnName("processed_on_utc");
		builder.Property(o => o.OccurredOnUtc);//.HasColumnName("occurred_on_utc");
		builder.Property(o => o.Error);//.HasColumnName("error");

		builder.HasMany<InboxMessageConsumer>()
			.WithOne()
			.HasForeignKey(x => x.InboxMessageId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
