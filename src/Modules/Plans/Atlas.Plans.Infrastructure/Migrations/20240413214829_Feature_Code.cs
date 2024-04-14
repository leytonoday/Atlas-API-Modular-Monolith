using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Atlas.Plans.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Feature_Code : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "Plans",
                table: "Features",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Plans",
                table: "Features");
        }
    }
}
