using System.ComponentModel.DataAnnotations.Schema;

namespace ThreadLike.Common.Domain;

public abstract class Entity
{
	[NotMapped]
	private readonly HashSet<IDomainEvent> _domainEvents = new();
	[NotMapped]
	public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.ToList();

	protected Entity() { }
	protected void Raise(IDomainEvent domainEvent)
	{
		_domainEvents.Add(domainEvent);
	}
	public void ClearDomainEvents()
	{
		_domainEvents.Clear();
	}
}
