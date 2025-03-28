using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThreadLike.User.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Update_Database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InboxMessageConsumers_InboxMessages_InboxMessageId",
                schema: "users",
                table: "InboxMessageConsumers");

            migrationBuilder.DropForeignKey(
                name: "FK_OutboxMessageConsumers_OutboxMessages_OutboxMessageId",
                schema: "users",
                table: "OutboxMessageConsumers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_InboxMessageConsumers_InboxMessages_InboxMessageId",
                schema: "users",
                table: "InboxMessageConsumers",
                column: "InboxMessageId",
                principalSchema: "users",
                principalTable: "InboxMessages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OutboxMessageConsumers_OutboxMessages_OutboxMessageId",
                schema: "users",
                table: "OutboxMessageConsumers",
                column: "OutboxMessageId",
                principalSchema: "users",
                principalTable: "OutboxMessages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
