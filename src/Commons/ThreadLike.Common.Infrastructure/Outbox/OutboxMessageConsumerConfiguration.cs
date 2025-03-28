﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ThreadLike.Common.Infrastructure.Outbox;

public sealed class OutboxMessageConsumerConfiguration : IEntityTypeConfiguration<OutboxMessageConsumer>
{
	public void Configure(EntityTypeBuilder<OutboxMessageConsumer> builder)
	{
		builder.ToTable("OutboxMessageConsumers");

		builder.HasKey(o => new { o.OutboxMessageId, o.Name });

		//builder.Property(o => o.Name).HasMaxLength(500);
	}
}
