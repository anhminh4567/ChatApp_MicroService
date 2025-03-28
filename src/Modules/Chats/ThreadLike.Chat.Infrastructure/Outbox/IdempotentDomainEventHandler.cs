using System.Data.Common;
using Dapper;
using MediatR;
using ThreadLike.Chat.Infrastructure.Database;
using ThreadLike.Common.Application.Data;
using ThreadLike.Common.Domain;
using ThreadLike.Common.Infrastructure.Outbox;

namespace ThreadLike.Chat.Infrastructure.Outbox;


// This is a decoreated class for the DOmainEventHandler<T> 
// we do this in dependency injection
internal sealed class IdempotentDomainEventHandler<TDomainEvent> : INotificationHandler<TDomainEvent>
	where TDomainEvent : IDomainEvent
{
	private readonly INotificationHandler<TDomainEvent> decorated;
	private readonly IDbConnectionFactory dbConnectionFactory;
	private readonly ChatDbContext dbContext;

	public IdempotentDomainEventHandler(INotificationHandler<TDomainEvent> decorated, IDbConnectionFactory dbConnectionFactory, ChatDbContext dbContext)
	{
		this.decorated = decorated;
		this.dbConnectionFactory = dbConnectionFactory;
		this.dbContext = dbContext;
	}

	public async Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken = default)
	{
		await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

		var outboxMessageConsumer = new OutboxMessageConsumer(Guid.Parse(domainEvent.Id), decorated.GetType().Name);
		// we check here IF we have consume this message yet ??? ( from OutboxMessageConsumer )
		if (await OutboxConsumerExistsAsync(connection, outboxMessageConsumer))
		{
			return;
		}

		await decorated.Handle(domainEvent, cancellationToken);
		// if we consumer success => insert into table "OutboxMessageConsumer"
		await InsertOutboxConsumerAsync(connection, outboxMessageConsumer);
	}

	private static async Task<bool> OutboxConsumerExistsAsync(
		DbConnection dbConnection,
		OutboxMessageConsumer outboxMessageConsumer)
	{
		const string sql =
			"""
            SELECT EXISTS(
                SELECT 1
                FROM chat."OutboxMessageConsumers"
                WHERE "OutboxMessageId" = @OutboxMessageId AND
                      "Name" = @Name
            )
            """;

		return await dbConnection.ExecuteScalarAsync<bool>(sql, outboxMessageConsumer);
	}

	private static async Task InsertOutboxConsumerAsync(
		DbConnection dbConnection,
		OutboxMessageConsumer outboxMessageConsumer)
	{
		const string sql =
			"""
            INSERT INTO chat."OutboxMessageConsumers"("OutboxMessageId", "Name")
            VALUES (@OutboxMessageId, @Name)
            """;

		await dbConnection.ExecuteAsync(sql, outboxMessageConsumer);
	}
}
