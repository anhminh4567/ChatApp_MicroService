using MediatR;

namespace ThreadLike.Common.Domain;

public interface IDomainEvent : INotification
{
	string Id { get; }
	DateTime OccuredTimeUtc { get; }
}
