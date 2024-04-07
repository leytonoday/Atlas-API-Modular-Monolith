using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Atlas.Law.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LegalDocumentSummary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Keywords",
                schema: "Law",
                table: "LegalDocuments");

            migrationBuilder.DropColumn(
                name: "ProcessingStatus",
                schema: "Law",
                table: "LegalDocuments");

            migrationBuilder.DropColumn(
                name: "SummarisedText",
                schema: "Law",
                table: "LegalDocuments");

            migrationBuilder.DropColumn(
                name: "SummarizedTitle",
                schema: "Law",
                table: "LegalDocuments");

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
                name: "IX_LegalDocumentSummaries_LegalDocumentId",
                schema: "Law",
                table: "LegalDocumentSummaries",
                column: "LegalDocumentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LegalDocumentSummaries",
                schema: "Law");

            migrationBuilder.AddColumn<string>(
                name: "Keywords",
                schema: "Law",
                table: "LegalDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProcessingStatus",
                schema: "Law",
                table: "LegalDocuments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SummarisedText",
                schema: "Law",
                table: "LegalDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SummarizedTitle",
                schema: "Law",
                table: "LegalDocuments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
