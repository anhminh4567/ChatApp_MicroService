using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Application.Groups.Dtos;
using ThreadLike.Chat.Domain.Groups;
using ThreadLike.Chat.Domain.Groups.Entities;
using ThreadLike.Chat.Domain.Users;
using ThreadLike.Common.Application.Data;
using ThreadLike.Common.Application.Messaging;
using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.Application.Groups.Queries
{
	public record GetGroupDetailQuery(Guid GroupId) : IQuery<Result<GroupDetailDto>>;
	internal class GetGroupDetailQueryHandler(
		IDbConnectionFactory connectionFactory) : IQueryHandler<GetGroupDetailQuery, Result<GroupDetailDto>>
	{
		public async Task<Result<GroupDetailDto>> Handle(GetGroupDetailQuery request, CancellationToken cancellationToken)
		{
			string groupParticipantSql = """
                                        WITH FoundGroup as (SELECT 
                                            g."Id",
                                            g."Name",
                                            g."CreatorId", 
                                            g."CreatedAt", 
                                            g."DeleteAt", 
                                            g."GroupType", 
                                            g."ThumbDetail"
                                            FROM chat."Groups" g
                                            WHERE g."Id" = @GroupId 
                                        )
                                        SELECT fg."Id",
                                            fg."Name",
                                            fg."CreatorId", 
                                            fg."CreatedAt", 
                                            fg."DeleteAt", 
                                            fg."GroupType", 
                                            fg."ThumbDetail",
                                            p."UserId", 
                                            p."GroupId", 
                                            p."RoleName", 
                                            p."IsMuted", 
                                            p."JoinedAt"
                                        FROM FoundGroup fg
                                        LEFT JOIN chat."Participants" p on fg."Id" = p."GroupId";
                                        """;
			string userSql = """
							SELECT 
							"Id",
							"Name", 
							"Email", 
							"IdentityId", 
							"AvatarUri", 
							"CreatedAt", 
							"UpdatedAt", 
							"IsVerified"
							FROM chat."Users" WHERE "Id" IN 
								(SELECT "UserId" FROM chat."Participants" WHERE "GroupId" = @GroupId);
							""";


			using DbConnection connection = await connectionFactory.OpenConnectionAsync(cancellationToken);

			Dictionary<Guid, Group> groupDictionary = new();

			IEnumerable<Group> groupParticpants = await connection.QueryAsync<Group, Participant, Group>(
				groupParticipantSql,
				(group, participant) =>
				{
					if (!groupDictionary.TryGetValue(group.Id, out Group? groupEntry))
					{
						groupEntry = group;
						groupDictionary.Add(groupEntry.Id, groupEntry);
					}
					if (participant != null && participant.UserId != null && participant.RoleName != null)
						groupEntry.Participants.Add(participant);
					return groupEntry;
				},
				new { GroupId = request.GroupId },
				splitOn: "UserId");

			Group? foundedGroup = groupParticpants.FirstOrDefault();
			if (foundedGroup == null)
				return Result.Failure(GroupErrors.NotFound);

			IEnumerable<User> users = await connection.QueryAsync<User>(userSql, new { request.GroupId });

			var groupDetailDto = new GroupDetailDto
			{
				Group = foundedGroup,
				ParticipantsDetail = users.ToList()
			};

			return Result.Success(groupDetailDto);
		}
	}
}
