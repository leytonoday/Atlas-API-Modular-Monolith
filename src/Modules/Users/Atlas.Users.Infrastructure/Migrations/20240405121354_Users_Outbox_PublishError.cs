using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Atlas.Users.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Users_Outbox_PublishError : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublishError",
                schema: "Users",
                table: "OutboxMessages",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublishError",
                schema: "Users",
                table: "OutboxMessages");
        }
    }
}
