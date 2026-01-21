using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eBoardAPI.Migrations
{
    /// <inheritdoc />
    public partial class NotificationIsRead : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "ParentNotifications",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "ParentNotifications");
        }
    }
}
