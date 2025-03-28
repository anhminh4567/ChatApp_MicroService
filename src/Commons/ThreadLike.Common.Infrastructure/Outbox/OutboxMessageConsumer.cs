namespace ThreadLike.Common.Infrastructure.Outbox;

public sealed class OutboxMessageConsumer(Guid outboxMessageId, string name)
{
	public Guid OutboxMessageId { get; init; } = outboxMessageId;
	/// <summary>
	/// this is the name of the Handler
	/// </summary>
	public string Name { get; init; } = name;
}
