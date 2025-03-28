using System.Data.Common;
using Dapper;
using MassTransit;
using Newtonsoft.Json;
using ThreadLike.Common.Application.Data;
using ThreadLike.Common.Contracts.Abstracts;
using ThreadLike.Common.Infrastructure.Inbox;
using ThreadLike.Common.Infrastructure.Serialization;

namespace ThreadLike.Chat.Infrastructure.Inbox;

public class IntegrationEventConsumer<TIntegrationEvent> : IConsumer<TIntegrationEvent>
	where TIntegrationEvent : IntegrationEvent
{
	private readonly IDbConnectionFactory dbConnectionFactory;

	public IntegrationEventConsumer(IDbConnectionFactory dbConnectionFactory)
	{
		this.dbConnectionFactory = dbConnectionFactory;
	}

	public async Task Consume(ConsumeContext<TIntegrationEvent> context)
	{
		await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

		TIntegrationEvent integrationEvent = context.Message;

		var inboxMessage = new InboxMessage
		{
			Id = Guid.Parse(integrationEvent.Id),
			Type = integrationEvent.GetType().Name,
			Content = JsonConvert.SerializeObject(integrationEvent, SerializerSettings.Instance),
			OccurredOnUtc = integrationEvent.OccurredOnUtc
		};

		const string sql =
			"""
            INSERT INTO chat."InboxMessages"("Id", "Type", "Content", "OccurredOnUtc")
            VALUES (@Id, @Type, @Content::json, @OccurredOnUtc)
            """;

		await connection.ExecuteAsync(sql, inboxMessage);
	}
}
