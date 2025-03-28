using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ThreadLike.Chat.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Create_Database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "chat");

            migrationBuilder.CreateTable(
                name: "GroupRoles",
                schema: "chat",
                columns: table => new
                {
                    Role = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupRoles", x => x.Role);
                });

            migrationBuilder.CreateTable(
                name: "InboxMessages",
                schema: "chat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "jsonb", nullable: false),
                    OccurredOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProcessedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Error = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboxMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessages",
                schema: "chat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "jsonb", nullable: false),
                    OccurredOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProcessedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Error = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reactions",
                schema: "chat",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ReactionDescription = table.Column<string>(type: "text", nullable: false),
                    MimeType = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "chat",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    IdentityId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AvatarUri = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InboxMessageConsumers",
                schema: "chat",
                columns: table => new
                {
                    InboxMessageId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboxMessageConsumers", x => new { x.InboxMessageId, x.Name });
                    table.ForeignKey(
                        name: "FK_InboxMessageConsumers_InboxMessages_InboxMessageId",
                        column: x => x.InboxMessageId,
                        principalSchema: "chat",
                        principalTable: "InboxMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessageConsumers",
                schema: "chat",
                columns: table => new
                {
                    OutboxMessageId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessageConsumers", x => new { x.OutboxMessageId, x.Name });
                    table.ForeignKey(
                        name: "FK_OutboxMessageConsumers_OutboxMessages_OutboxMessageId",
                        column: x => x.OutboxMessageId,
                        principalSchema: "chat",
                        principalTable: "OutboxMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                schema: "chat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatorId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalSchema: "chat",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Message",
                schema: "chat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SenderId = table.Column<string>(type: "text", nullable: false),
                    ReceiverId = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    ReferenceId = table.Column<Guid>(type: "uuid", nullable: true),
                    RefrenceMessageId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Message_Message_RefrenceMessageId",
                        column: x => x.RefrenceMessageId,
                        principalSchema: "chat",
                        principalTable: "Message",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Message_Users_SenderId",
                        column: x => x.SenderId,
                        principalSchema: "chat",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Message_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "chat",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserFriends",
                schema: "chat",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    FriendId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFriends", x => new { x.FriendId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserFriends_Users_FriendId",
                        column: x => x.FriendId,
                        principalSchema: "chat",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserFriends_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "chat",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserLetters",
                schema: "chat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SenderId = table.Column<string>(type: "text", nullable: true),
                    ReceiverId = table.Column<string>(type: "text", nullable: false),
                    LetterType = table.Column<int>(type: "integer", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    SentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsHtml = table.Column<bool>(type: "boolean", nullable: false),
                    ExpireTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLetters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLetters_Users_ReceiverId",
                        column: x => x.ReceiverId,
                        principalSchema: "chat",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLetters_Users_SenderId",
                        column: x => x.SenderId,
                        principalSchema: "chat",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupMessage",
                schema: "chat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    SenderId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    ReferenceId = table.Column<Guid>(type: "uuid", nullable: true),
                    RefrenceMessageId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupMessage_GroupMessage_ReferenceId",
                        column: x => x.ReferenceId,
                        principalSchema: "chat",
                        principalTable: "GroupMessage",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GroupMessage_GroupMessage_RefrenceMessageId",
                        column: x => x.RefrenceMessageId,
                        principalSchema: "chat",
                        principalTable: "GroupMessage",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GroupMessage_Groups_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "chat",
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupMessage_Users_SenderId",
                        column: x => x.SenderId,
                        principalSchema: "chat",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserGroupRoles",
                schema: "chat",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    GroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroupRoles", x => new { x.UserId, x.GroupId, x.RoleName });
                    table.ForeignKey(
                        name: "FK_UserGroupRoles_GroupRoles_RoleName",
                        column: x => x.RoleName,
                        principalSchema: "chat",
                        principalTable: "GroupRoles",
                        principalColumn: "Role",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UserGroupRoles_Groups_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "chat",
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGroupRoles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "chat",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MessageReaction",
                schema: "chat",
                columns: table => new
                {
                    MesssageId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReactionId = table.Column<string>(type: "text", nullable: false),
                    MessageId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReactorIds = table.Column<List<string>>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageReaction", x => new { x.MesssageId, x.ReactionId });
                    table.ForeignKey(
                        name: "FK_MessageReaction_Message_MessageId",
                        column: x => x.MessageId,
                        principalSchema: "chat",
                        principalTable: "Message",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MessageReaction_Reactions_ReactionId",
                        column: x => x.ReactionId,
                        principalSchema: "chat",
                        principalTable: "Reactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupMessageReaction",
                schema: "chat",
                columns: table => new
                {
                    MesssageId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReactionId = table.Column<string>(type: "text", nullable: false),
                    GroupMessageId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReactorIds = table.Column<List<string>>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMessageReaction", x => new { x.MesssageId, x.ReactionId });
                    table.ForeignKey(
                        name: "FK_GroupMessageReaction_GroupMessage_GroupMessageId",
                        column: x => x.GroupMessageId,
                        principalSchema: "chat",
                        principalTable: "GroupMessage",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GroupMessageReaction_Reactions_ReactionId",
                        column: x => x.ReactionId,
                        principalSchema: "chat",
                        principalTable: "Reactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "chat",
                table: "GroupRoles",
                columns: new[] { "Role", "Description", "IsDefault", "IsSystem" },
                values: new object[,]
                {
                    { "GroupLeader", "GroupLeader", true, true },
                    { "Guest", "Guest", true, true },
                    { "Member", "Member", true, true }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessage_GroupId",
                schema: "chat",
                table: "GroupMessage",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessage_Id_GroupId",
                schema: "chat",
                table: "GroupMessage",
                columns: new[] { "Id", "GroupId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessage_ReferenceId",
                schema: "chat",
                table: "GroupMessage",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessage_RefrenceMessageId",
                schema: "chat",
                table: "GroupMessage",
                column: "RefrenceMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessage_SenderId",
                schema: "chat",
                table: "GroupMessage",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessageReaction_GroupMessageId",
                schema: "chat",
                table: "GroupMessageReaction",
                column: "GroupMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessageReaction_ReactionId",
                schema: "chat",
                table: "GroupMessageReaction",
                column: "ReactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_CreatorId",
                schema: "chat",
                table: "Groups",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_RefrenceMessageId",
                schema: "chat",
                table: "Message",
                column: "RefrenceMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_SenderId",
                schema: "chat",
                table: "Message",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_UserId",
                schema: "chat",
                table: "Message",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReaction_MessageId",
                schema: "chat",
                table: "MessageReaction",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReaction_ReactionId",
                schema: "chat",
                table: "MessageReaction",
                column: "ReactionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFriends_UserId",
                schema: "chat",
                table: "UserFriends",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupRoles_GroupId",
                schema: "chat",
                table: "UserGroupRoles",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupRoles_RoleName",
                schema: "chat",
                table: "UserGroupRoles",
                column: "RoleName");

            migrationBuilder.CreateIndex(
                name: "IX_UserLetters_ReceiverId",
                schema: "chat",
                table: "UserLetters",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLetters_SenderId",
                schema: "chat",
                table: "UserLetters",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdentityId",
                schema: "chat",
                table: "Users",
                column: "IdentityId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupMessageReaction",
                schema: "chat");

            migrationBuilder.DropTable(
                name: "InboxMessageConsumers",
                schema: "chat");

            migrationBuilder.DropTable(
                name: "MessageReaction",
                schema: "chat");

            migrationBuilder.DropTable(
                name: "OutboxMessageConsumers",
                schema: "chat");

            migrationBuilder.DropTable(
                name: "UserFriends",
                schema: "chat");

            migrationBuilder.DropTable(
                name: "UserGroupRoles",
                schema: "chat");

            migrationBuilder.DropTable(
                name: "UserLetters",
                schema: "chat");

            migrationBuilder.DropTable(
                name: "GroupMessage",
                schema: "chat");

            migrationBuilder.DropTable(
                name: "InboxMessages",
                schema: "chat");

            migrationBuilder.DropTable(
                name: "Message",
                schema: "chat");

            migrationBuilder.DropTable(
                name: "Reactions",
                schema: "chat");

            migrationBuilder.DropTable(
                name: "OutboxMessages",
                schema: "chat");

            migrationBuilder.DropTable(
                name: "GroupRoles",
                schema: "chat");

            migrationBuilder.DropTable(
                name: "Groups",
                schema: "chat");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "chat");
        }
    }
}
