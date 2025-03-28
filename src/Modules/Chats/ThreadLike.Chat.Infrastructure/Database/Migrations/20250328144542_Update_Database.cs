using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThreadLike.Chat.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Update_Database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InboxMessageConsumers_InboxMessages_InboxMessageId",
                schema: "chat",
                table: "InboxMessageConsumers");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageAttachment_Message_MessageId",
                schema: "chat",
                table: "MessageAttachment");

            migrationBuilder.DropForeignKey(
                name: "FK_OutboxMessageConsumers_OutboxMessages_OutboxMessageId",
                schema: "chat",
                table: "OutboxMessageConsumers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MessageAttachment",
                schema: "chat",
                table: "MessageAttachment");

            migrationBuilder.RenameTable(
                name: "MessageAttachment",
                schema: "chat",
                newName: "MessagesAttachments",
                newSchema: "chat");

            migrationBuilder.RenameIndex(
                name: "IX_MessageAttachment_MessageId",
                schema: "chat",
                table: "MessagesAttachments",
                newName: "IX_MessagesAttachments_MessageId");

            migrationBuilder.AlterColumn<string>(
                name: "AvatarUri",
                schema: "chat",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessagesAttachments",
                schema: "chat",
                table: "MessagesAttachments",
                columns: new[] { "Id", "MessageId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MessagesAttachments_Message_MessageId",
                schema: "chat",
                table: "MessagesAttachments",
                column: "MessageId",
                principalSchema: "chat",
                principalTable: "Message",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessagesAttachments_Message_MessageId",
                schema: "chat",
                table: "MessagesAttachments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MessagesAttachments",
                schema: "chat",
                table: "MessagesAttachments");

            migrationBuilder.RenameTable(
                name: "MessagesAttachments",
                schema: "chat",
                newName: "MessageAttachment",
                newSchema: "chat");

            migrationBuilder.RenameIndex(
                name: "IX_MessagesAttachments_MessageId",
                schema: "chat",
                table: "MessageAttachment",
                newName: "IX_MessageAttachment_MessageId");

            migrationBuilder.AlterColumn<string>(
                name: "AvatarUri",
                schema: "chat",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessageAttachment",
                schema: "chat",
                table: "MessageAttachment",
                columns: new[] { "Id", "MessageId" });

            migrationBuilder.AddForeignKey(
                name: "FK_InboxMessageConsumers_InboxMessages_InboxMessageId",
                schema: "chat",
                table: "InboxMessageConsumers",
                column: "InboxMessageId",
                principalSchema: "chat",
                principalTable: "InboxMessages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageAttachment_Message_MessageId",
                schema: "chat",
                table: "MessageAttachment",
                column: "MessageId",
                principalSchema: "chat",
                principalTable: "Message",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OutboxMessageConsumers_OutboxMessages_OutboxMessageId",
                schema: "chat",
                table: "OutboxMessageConsumers",
                column: "OutboxMessageId",
                principalSchema: "chat",
                principalTable: "OutboxMessages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
