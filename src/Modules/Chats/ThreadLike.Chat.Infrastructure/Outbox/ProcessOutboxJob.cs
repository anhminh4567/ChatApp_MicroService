using System.Data;
using System.Data.Common;
using System.Diagnostics;
using Dapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;
using ThreadLike.Common.Application.Data;
using ThreadLike.Common.Domain;
using ThreadLike.Common.Infrastructure.Outbox;
using ThreadLike.Common.Infrastructure.Serialization;

namespace ThreadLike.Chat.Infrastructure.Outbox;

[DisallowConcurrentExecution]
internal sealed class ProcessOutboxJob : IJob
{
	private const string ModuleName = "Chat";
	private readonly IDbConnectionFactory _dbConnectionFactory;
	private readonly IServiceScopeFactory _serviceScopeFactory;
	private readonly IOptions<OutboxOptions> _outboxOptions;
	private readonly ILogger<ProcessOutboxJob> _logger;
	private readonly IPublisher _publisher;

	public ProcessOutboxJob(IDbConnectionFactory dbConnectionFactory, IServiceScopeFactory serviceScopeFactory, IOptions<OutboxOptions> outboxOptions, ILogger<ProcessOutboxJob> logger, IPublisher publisher)
	{
		_dbConnectionFactory = dbConnectionFactory;
		_serviceScopeFactory = serviceScopeFactory;
		_outboxOptions = outboxOptions;
		_logger = logger;
		_publisher = publisher;
	}

	public async Task Execute(IJobExecutionContext context)
	{
		using var activity = new Activity(ModuleName + "."+ OutboxOptions.SectionName);
		activity.Start();
		activity.AddTag("module", ModuleName);
		activity.AddTag("batchSize", _outboxOptions.Value.BatchSize.ToString());

		_logger.LogInformation("{Module} - Beginning to process outbox messages", ModuleName);

		await using DbConnection connection = await _dbConnectionFactory.OpenConnectionAsync();
		await using DbTransaction transaction = await connection.BeginTransactionAsync();


		using IServiceScope scope = _serviceScopeFactory.CreateScope();

		IReadOnlyList<OutboxMessage> outboxMessages = await GetOutboxMessagesAsync(connection, transaction);


		foreach (OutboxMessage outboxMessage in outboxMessages)
		{
			Exception? exception = null;
			try
			{
				IDomainEvent domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(
					outboxMessage.Content,
					SerializerSettings.Instance)!;
				Type getType = domainEvent.GetType();
				//using IServiceScope scope = _serviceScopeFactory.CreateScope();
				await _publisher.Publish(domainEvent);

				// ---------------------------------------- new publish method  -------------------------------------------//
				//IEnumerable<IDomainEventHandler> domainEventHandler = DomainEventHandlersFactory.GetHandlers(domainEvent.GetType(), scope.ServiceProvider, Application.AssemblyReference.Assembly);
				//foreach (var handler in domainEventHandler)
				//{
				//	await handler.Handle(domainEvent);
				//}
				// ---------------------------------------- new publish method  -------------------------------------------//

			}
			catch (Exception caughtException)
			{
				_logger.LogError(
					caughtException,
					"{Module} - Exception while processing outbox message {MessageId}",
					ModuleName,
					outboxMessage.Id);

				exception = caughtException;
			}

			await UpdateOutboxMessageAsync(connection, transaction, outboxMessage, exception);
		}
		await transaction.CommitAsync();

		_logger.LogInformation("{Module} - Completed processing outbox messages", ModuleName);

		activity.Stop();
	}

	private async Task<IReadOnlyList<OutboxMessage>> GetOutboxMessagesAsync(
	IDbConnection connection,
	IDbTransaction transaction)
	{
		string sql =
			$"""
             SELECT
                "Id" AS {nameof(OutboxMessage.Id)},
                "Content" AS {nameof(OutboxMessage.Content)},
                "OccurredOnUtc" AS {nameof(OutboxMessage.OccurredOnUtc)},
                "ProcessedOnUtc" AS {nameof(OutboxMessage.ProcessedOnUtc)},
                "Error" AS {nameof(OutboxMessage.Error)},  "Type" AS {nameof(OutboxMessage.Type)}
             FROM chat."OutboxMessages"
             WHERE "ProcessedOnUtc" IS NULL
             ORDER BY "OccurredOnUtc"
             LIMIT {_outboxOptions.Value.BatchSize}
             FOR UPDATE SKIP LOCKED;
             """;

		IEnumerable<OutboxMessage> outboxMessages = await connection.QueryAsync<OutboxMessage>(
		sql,
		transaction: transaction);
		return outboxMessages.ToList();
	}

	private async Task UpdateOutboxMessageAsync(
		IDbConnection connection,
		IDbTransaction transaction,
		OutboxMessage outboxMessage,
		Exception? exception)
	{
		const string sql =
			"""
            UPDATE chat."OutboxMessages"
            SET "ProcessedOnUtc" = @ProcessedOnUtc,
                "Error" = @Error
            WHERE "Id" = @Id
            """;

		await connection.ExecuteAsync(
			sql,
			new
			{
				outboxMessage.Id,
				ProcessedOnUtc = DateTime.UtcNow,
				Error = exception?.ToString()
			},
			transaction: transaction);
	}

	//internal sealed record OutboxMessageResponse(Guid Id, string Content, DateTime OccurredOnUtc, DateTime? ProcessedOnUtc, string? Error);
}
