using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.Messages;
using ThreadLike.Chat.Domain.Users;
using ThreadLike.Common.Application.Data;
using ThreadLike.Common.Application.Messaging;
using ThreadLike.Common.Domain;
using Dapper;
using ThreadLike.Chat.Domain.Messages.Entities;
using ThreadLike.Common.Domain.Shares;
using System.Globalization;
using ThreadLike.Common.Domain.Ultils;
namespace ThreadLike.Chat.Application.Messages.Queries
{
	// use Cursor paging since we already have the CreateAt field,
	// BUT WE DID NOT HAVE INDEX
	// go create index for column (groupId, createAt) in database
	public record GetMessagesPagingQuery(Guid GroupId, DateTime dateTimeCursor, int Take = 20) : IQuery<Result<List<Message>>>;
	internal class GetMessagesPagingQueryHandler(
		IDbConnectionFactory dbConnectionFactory) : IQueryHandler<GetMessagesPagingQuery, Result<List<Message>>>
	{
		public async Task<Result<List<Message>>> Handle(GetMessagesPagingQuery request, CancellationToken cancellationToken)
		{
			using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync(cancellationToken);
			request.Deconstruct(out var groupId, out DateTime dateTimeCursor, out int take);
			//if(DateTime.TryParse(request.DateTimeCursor,null,out DateTime dateTimeCursor))//DateTimeUtils.TryParseDateTimeToAllowedRules(request.DateTimeCursor, out DateTime dateTimeCursor) == false
			//{
			//	return Result.Failure("Date and time format are not allowed");
			//}
			string query = """
				WITH FilteredMessages AS (
				    SELECT "Id", "SenderId", "GroupId", "Content", "ReferenceId", "CreatedAt", "DeletedAt"
				    FROM chat."Message"
				    WHERE "GroupId" = @groupId
				    AND "CreatedAt" <= @dateTimeCursor
				    ORDER BY "CreatedAt" DESC
				    LIMIT @take
				)
				SELECT 
				    m."Id", 
				    m."SenderId", 
				    m."GroupId", 
				    m."Content", 
				    m."ReferenceId", 
				    m."CreatedAt", 
				    m."DeletedAt",
				    a."Id" AS AttachmentId,
					a."Id" AS Id,
				    a."MessageId", --AS AttachmentMessageId,
				    a."AttachmentDetail",
				    a."ThumbDetail",
				    r."MesssageId" AS ReactionMessageId,
				    r."MesssageId" AS MessageId,
					r."ReactionId",
				    r."ReactorIds"
				FROM FilteredMessages m
				LEFT JOIN chat."MessagesAttachments" a ON m."Id" = a."MessageId"
				LEFT JOIN chat."MessageReaction" r ON m."Id" = r."MesssageId"
				ORDER BY m."CreatedAt" DESC;
				""";
			var resultMessages = new Dictionary<Guid, Message>();

			TimeSpan offset = TimeZoneInfo.Local.GetUtcOffset(dateTimeCursor);
			DateTimeOffset dateTimeCursorOffset = new DateTimeOffset(dateTimeCursor, offset);
			DateTime utcDateTimeCursor = dateTimeCursor.ToUniversalTime();//.AddSeconds(1);
			IEnumerable<Message> result = await connection
				.QueryAsync<Message, MessageAttachment, MessageReaction, Message>(
				query,
				(currentMessage, attachement, reaction) =>
				{
					if(attachement != null && attachement.Id != null)
					{
						currentMessage.MessageAttachments.Add(attachement);
					}
					if (reaction != null && reaction.ReactorIds.Count != 0)
					{
						currentMessage.MessageReactions.Add(reaction);
					}
					return currentMessage;
				},
				new { groupId = request.GroupId, dateTimeCursor = utcDateTimeCursor, take = request.Take },
				splitOn: "AttachmentId,ReactionMessageId");

			HashSet<Message> hashsetResult = result.ToHashSet(new MessageIdEqualityComparer());
			return hashsetResult.ToList();
		}
	}
}
