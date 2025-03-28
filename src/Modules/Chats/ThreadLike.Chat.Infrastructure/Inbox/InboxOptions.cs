namespace ThreadLike.Chat.Infrastructure.Inbox;

internal sealed class InboxOptions
{
	public const string SectionName = "Inbox";
	public int IntervalInSeconds { get; init; }

	public int BatchSize { get; init; }
}
