using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Atlas.Law.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LegalDocument_Summary_One_to_One : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LegalDocumentSummaries_LegalDocuments_LegalDocumentId",
                schema: "Law",
                table: "LegalDocumentSummaries");

            migrationBuilder.DropIndex(
                name: "IX_LegalDocumentSummaries_LegalDocumentId",
                schema: "Law",
                table: "LegalDocumentSummaries");

            migrationBuilder.CreateIndex(
                name: "IX_LegalDocumentSummaries_LegalDocumentId",
                schema: "Law",
                table: "LegalDocumentSummaries",
                column: "LegalDocumentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LegalDocumentSummaries_LegalDocuments_LegalDocumentId",
                schema: "Law",
                table: "LegalDocumentSummaries",
                column: "LegalDocumentId",
                principalSchema: "Law",
                principalTable: "LegalDocuments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LegalDocumentSummaries_LegalDocuments_LegalDocumentId",
                schema: "Law",
                table: "LegalDocumentSummaries");

            migrationBuilder.DropIndex(
                name: "IX_LegalDocumentSummaries_LegalDocumentId",
                schema: "Law",
                table: "LegalDocumentSummaries");

            migrationBuilder.CreateIndex(
                name: "IX_LegalDocumentSummaries_LegalDocumentId",
                schema: "Law",
                table: "LegalDocumentSummaries",
                column: "LegalDocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_LegalDocumentSummaries_LegalDocuments_LegalDocumentId",
                schema: "Law",
                table: "LegalDocumentSummaries",
                column: "LegalDocumentId",
                principalSchema: "Law",
                principalTable: "LegalDocuments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
