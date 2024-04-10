using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Atlas.Law.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LegalDocument_MimeType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MimeType",
                schema: "Law",
                table: "LegalDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MimeType",
                schema: "Law",
                table: "LegalDocuments");
        }
    }
}
