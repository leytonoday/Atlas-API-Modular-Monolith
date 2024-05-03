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
                name: "EurLexSumDocuments",
                schema: "Law",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CelexId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Keywords = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EurLexSumDocuments", x => x.Id);
                });

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
                name: "LegalDocuments",
                schema: "Law",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MimeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalDocuments", x => x.Id);
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

            migrationBuilder.CreateTable(
                name: "LegalDocumentSummaries",
                schema: "Law",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SummarisedText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SummarizedTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Keywords = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LegalDocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProcessingStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalDocumentSummaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LegalDocumentSummaries_LegalDocuments_LegalDocumentId",
                        column: x => x.LegalDocumentId,
                        principalSchema: "Law",
                        principalTable: "LegalDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EurLexSumDocuments_CelexId_Language",
                schema: "Law",
                table: "EurLexSumDocuments",
                columns: new[] { "CelexId", "Language" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LegalDocumentSummaries_LegalDocumentId",
                schema: "Law",
                table: "LegalDocumentSummaries",
                column: "LegalDocumentId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EurLexSumDocuments",
                schema: "Law");

            migrationBuilder.DropTable(
                name: "InboxMessageHandlerAcknowledgements",
                schema: "Law");

            migrationBuilder.DropTable(
                name: "InboxMessages",
                schema: "Law");

            migrationBuilder.DropTable(
                name: "LegalDocumentSummaries",
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

            migrationBuilder.DropTable(
                name: "LegalDocuments",
                schema: "Law");
        }
    }
}
