using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using ThreadLike.Common.Domain;
using ThreadLike.Common.Infrastructure.Serialization;

namespace ThreadLike.Common.Infrastructure.Outbox;

public sealed class InsertOutboxMessageEventsInterceptor : SaveChangesInterceptor //(IServiceScopeFactory serviceScopeFactory)
{
	private readonly IServiceProvider _serviceProvider;

	public InsertOutboxMessageEventsInterceptor(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}
	/// <summary>
	/// THIS IS FOR OUTBOX MESSAGE, Saving changes happen before commit transaction, so the message will be in the same scope as the orginal transaction
	/// </summary>
	/// <param name="eventData"></param>
	/// <param name="result"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
	{
		if (eventData.Context is not null)
		{
			InsertOutboxMessage(eventData.Context);
		}
		return await base.SavingChangesAsync(eventData, result, cancellationToken);
	}

	/// <summary>
	/// this is for when we still use PublishDomainEvent ( not using outbox yet )
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>

	//public override async ValueTask<int> SavedChangesAsync(
	//    SaveChangesCompletedEventData eventData,
	//    int result,
	//    CancellationToken cancellationToken = default)
	//{
	//    if (eventData.Context is not null)
	//    {
	//        await PublishDomainEventsAsync(eventData.Context);
	//    }

	//    return await base.SavedChangesAsync(eventData, result, cancellationToken);
	//}

	private static void InsertOutboxMessage(DbContext context)
	{

		var outboxMessages = context
			.ChangeTracker
			.Entries<Entity>()
			.Select(entry => entry.Entity)
			.SelectMany(entity =>
			{
				IReadOnlyCollection<IDomainEvent> domainEvents = entity.DomainEvents;
				entity.ClearDomainEvents();
				return domainEvents;
			})
			.Select(domainEvent =>
			 new OutboxMessage
			 {
				 Id = Guid.Parse(domainEvent.Id),
				 OccurredOnUtc = domainEvent.OccuredTimeUtc,
				 Type = domainEvent.GetType().Name,
				 Content = JsonConvert.SerializeObject(domainEvent, SerializerSettings.Instance)
			 })
			.ToList();
		context.Set<OutboxMessage>().AddRange(outboxMessages);
		//var domainEvents = context
		//    .ChangeTracker
		//    .Entries<Entity>()
		//    .Select(entry => entry.Entity)
		//    .SelectMany(entity =>
		//    {
		//        IReadOnlyCollection<IDomainEvent> domainEvents = entity.DomainEvents;
		//        entity.ClearDomainEvents();
		//        return domainEvents;
		//    })

		//    .ToList();

		//using IServiceScope scope = _serviceProvider.CreateScope();
		//IPublisher publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();
		//foreach (IDomainEvent domainEvent in domainEvents)
		//{
		//    await publisher.Publish(domainEvent);
		//}
	}
}
