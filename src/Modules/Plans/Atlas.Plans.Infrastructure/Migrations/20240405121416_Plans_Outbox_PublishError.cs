using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Atlas.Plans.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Plans_Outbox_PublishError : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublishError",
                schema: "Plans",
                table: "OutboxMessages",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublishError",
                schema: "Plans",
                table: "OutboxMessages");
        }
    }
}
