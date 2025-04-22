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
	/// <summary>
	/// some event are fast process and dont have to be persist, if so an interceptor
	/// relative to the module will handle this before the transaction commit
	/// </summary>
	public void RemoveEvent(string eventId)
	{
		_domainEvents.RemoveWhere(e => e.Id == eventId);
	}
}
