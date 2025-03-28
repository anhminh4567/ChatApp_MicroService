namespace ThreadLike.Common.Domain;

public abstract record DomainEvent : IDomainEvent
{
	public string Id { get; init; }

	public DateTime OccuredTimeUtc { get; init; }

	protected DomainEvent()
	{
		Id = Guid.NewGuid().ToString();
		OccuredTimeUtc = DateTime.UtcNow;
	}
	protected DomainEvent(string id, DateTime occuredTimeUtc)
	{
		Id = id;
		OccuredTimeUtc = occuredTimeUtc;
	}

}
