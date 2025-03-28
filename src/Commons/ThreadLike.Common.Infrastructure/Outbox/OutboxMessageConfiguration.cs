using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ThreadLike.Common.Infrastructure.Outbox;

public sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
	public void Configure(EntityTypeBuilder<OutboxMessage> builder)
	{
		builder.ToTable("OutboxMessages");

		builder.HasKey(o => o.Id);

		builder.Property(o => o.Content).HasColumnType("jsonb");//.HasMaxLength(2000)

		//builder.HasMany<OutboxMessageConsumer>()
		//	.WithOne()
		//	.HasForeignKey(x => x.OutboxMessageId)
		//	.OnDelete(DeleteBehavior.Cascade);
	}
}
