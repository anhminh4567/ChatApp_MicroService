using System.Data;
using System.Data.Common;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;
using ThreadLike.Common.Application.Data;
using ThreadLike.Common.Application.EventBus;
using ThreadLike.Common.Contracts.Abstracts;
using ThreadLike.Common.Infrastructure.Inbox;
using ThreadLike.Common.Infrastructure.Serialization;
using ThreadLike.User.Application;

namespace ThreadLike.Chat.Infrastructure.Inbox;

[DisallowConcurrentExecution]
internal sealed class ProcessInboxJob(
	IDbConnectionFactory dbConnectionFactory,
	IServiceScopeFactory serviceScopeFactory,
	IOptions<InboxOptions> inboxOptions,
	ILogger<ProcessInboxJob> logger) : IJob
{
	private const string ModuleName = "User";

	public async Task Execute(IJobExecutionContext context)
	{
		logger.LogInformation("{Module} - Beginning to process inbox messages", ModuleName);

		await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();
		await using DbTransaction transaction = await connection.BeginTransactionAsync();

		IReadOnlyList<InboxMessageResponse> inboxMessages = await GetInboxMessagesAsync(connection, transaction);

		using IServiceScope scope = serviceScopeFactory.CreateScope();

		foreach (InboxMessageResponse inboxMessage in inboxMessages)
		{
			Exception? exception = null;

			try
			{
				IIntegrationEvent integrationEvent = JsonConvert.DeserializeObject<IIntegrationEvent>(
					inboxMessage.Content,
					SerializerSettings.Instance)!;


				IEnumerable<IIntegrationEventHandler> handlers = IntegrationEventHandlersFactory.GetHandlers(
					integrationEvent.GetType(),
					scope.ServiceProvider,
					typeof(ApplicationConfiguration).Assembly
					);
				// how handler now assume to be in application layer

				foreach (IIntegrationEventHandler integrationEventHandler in handlers)
				{
					await integrationEventHandler.Handle(integrationEvent, context.CancellationToken);
				}
			}
			catch (Exception caughtException)
			{
				logger.LogError(
					caughtException,
					"{Module} - Exception while processing inbox message {MessageId}",
					ModuleName,
					inboxMessage.Id);

				exception = caughtException;
			}

			await UpdateInboxMessageAsync(connection, transaction, inboxMessage, exception);
		}

		await transaction.CommitAsync();

		logger.LogInformation("{Module} - Completed processing inbox messages", ModuleName);
	}

	private async Task<IReadOnlyList<InboxMessageResponse>> GetInboxMessagesAsync(
		IDbConnection connection,
		IDbTransaction transaction)
	{
		string sql =
			$"""
             SELECT
                "Id" AS {nameof(InboxMessageResponse.Id)},
                "Content" AS {nameof(InboxMessageResponse.Content)}
             FROM users."InboxMessages"
             WHERE "ProcessedOnUtc" IS NULL
             ORDER BY "OccurredOnUtc"
             LIMIT {inboxOptions.Value.BatchSize}
             FOR UPDATE SKIP LOCKED
             """;

		IEnumerable<InboxMessageResponse> inboxMessages = await connection.QueryAsync<InboxMessageResponse>(
			sql,
			transaction: transaction);

		return inboxMessages.AsList();
	}

	private async Task UpdateInboxMessageAsync(
		IDbConnection connection,
		IDbTransaction transaction,
		InboxMessageResponse inboxMessage,
		Exception? exception)
	{
		const string sql =
			"""
            UPDATE users."InboxMessages"
            SET "ProcessedOnUtc" = @ProcessedOnUtc,
                "Error" = @Error
            WHERE "Id" = @Id
            """;

		await connection.ExecuteAsync(
			sql,
			new
			{
				inboxMessage.Id,
				ProcessedOnUtc = DateTime.UtcNow,
				Error = exception?.ToString()
			},
			transaction: transaction);
	}

	internal sealed record InboxMessageResponse(Guid Id, string Content);
}
