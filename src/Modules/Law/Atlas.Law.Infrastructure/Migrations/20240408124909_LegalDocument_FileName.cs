using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Atlas.Law.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LegalDocument_FileName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
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
                name: "Name",
                schema: "Law",
                table: "LegalDocuments");
        }
    }
}
