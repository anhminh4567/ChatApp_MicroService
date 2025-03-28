namespace ThreadLike.Common.Contracts.Abstracts;

public abstract record IntegrationEvent : IIntegrationEvent
{
	protected IntegrationEvent(string id, DateTime occurredOnUtc)
	{
		Id = id;
		OccurredOnUtc = occurredOnUtc;
	}

	public string Id { get; init; }

	public DateTime OccurredOnUtc { get; init; }
}
