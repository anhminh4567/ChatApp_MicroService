namespace ThreadLike.Common.Contracts.Abstracts;
public interface IIntegrationEvent
{
	string Id { get; }
	DateTime OccurredOnUtc { get; }
}

