namespace ThreadLike.Common.Domain;

/// <summary>
///This interface is used to mark events that are fast to process and can be handled in parallel.
///It is used to optimize the event processing pipeline.
/// The implementation of this interface should be lightweight and fast to process.
/// It should not contain any heavy logic or dependencies.
/// The implementation of this interface should be stateless and thread-safe.
/// </summary>
public interface IFastDomainEvent : IDomainEvent
{

}