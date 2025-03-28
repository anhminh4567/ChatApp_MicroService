using ThreadLike.Common.Domain;

namespace ThreadLike.User.Domain.Users.DomainEvents
{
	public record UserUpdatedDomainEvent(string userId) : DomainEvent
	{
		public string UpdatedUserId { get; init; } = userId;

	}
}