namespace ThreadLike.Chat.Infrastructure.Outbox;

internal sealed class OutboxOptions
{
	public const string SectionName = "Outbox";
	public int IntervalInSeconds { get; init; } 
	public int BatchSize { get; init; }
}
