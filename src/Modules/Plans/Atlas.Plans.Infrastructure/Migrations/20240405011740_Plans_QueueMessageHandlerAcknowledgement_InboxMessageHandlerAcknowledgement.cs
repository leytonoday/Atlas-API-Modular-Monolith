using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Atlas.Plans.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Plans_QueueMessageHandlerAcknowledgement_InboxMessageHandlerAcknowledgement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InboxMessageHandlerAcknowledgements",
                schema: "Plans",
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
                name: "QueueMessageHandlerAcknowledgements",
                schema: "Plans",
                columns: table => new
                {
                    QueuedCommandId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HandlerName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueueMessageHandlerAcknowledgements", x => new { x.HandlerName, x.QueuedCommandId });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InboxMessageHandlerAcknowledgements",
                schema: "Plans");

            migrationBuilder.DropTable(
                name: "QueueMessageHandlerAcknowledgements",
                schema: "Plans");
        }
    }
}
