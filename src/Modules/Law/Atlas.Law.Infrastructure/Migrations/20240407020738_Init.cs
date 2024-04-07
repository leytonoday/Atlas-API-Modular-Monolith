using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Atlas.Law.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Law");

            migrationBuilder.CreateTable(
                name: "InboxMessageHandlerAcknowledgements",
                schema: "Law",
                columns: table => new
                {
                    InboxMessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HandlerName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboxMessageHandlerAcknowledgements", x => new { x.HandlerName, x.InboxMessageId });
                });

            migrationBuilder.CreateTable(
                name: "InboxMessages",
                schema: "Law",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OccurredOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PublishError = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboxMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessages",
                schema: "Law",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OccurredOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PublishError = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QueueMessageHandlerAcknowledgements",
                schema: "Law",
                columns: table => new
                {
                    QueuedCommandId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HandlerName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueueMessageHandlerAcknowledgements", x => new { x.HandlerName, x.QueuedCommandId });
                });

            migrationBuilder.CreateTable(
                name: "QueueMessages",
                schema: "Law",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OccurredOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Error = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueueMessages", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InboxMessageHandlerAcknowledgements",
                schema: "Law");

            migrationBuilder.DropTable(
                name: "InboxMessages",
                schema: "Law");

            migrationBuilder.DropTable(
                name: "OutboxMessages",
                schema: "Law");

            migrationBuilder.DropTable(
                name: "QueueMessageHandlerAcknowledgements",
                schema: "Law");

            migrationBuilder.DropTable(
                name: "QueueMessages",
                schema: "Law");
        }
    }
}
