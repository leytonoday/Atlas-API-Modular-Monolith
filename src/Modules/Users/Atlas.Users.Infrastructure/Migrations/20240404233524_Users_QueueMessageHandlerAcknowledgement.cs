using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Atlas.Users.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Users_QueueMessageHandlerAcknowledgement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QueueMessageHandlerAcknowledgements",
                schema: "Users",
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
                name: "QueueMessageHandlerAcknowledgements",
                schema: "Users");
        }
    }
}
