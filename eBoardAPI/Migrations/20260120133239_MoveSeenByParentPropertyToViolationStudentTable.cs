using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eBoardAPI.Migrations
{
    /// <inheritdoc />
    public partial class MoveSeenByParentPropertyToViolationStudentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeenByParent",
                table: "Violations");

            migrationBuilder.AddColumn<bool>(
                name: "SeenByParent",
                table: "ViolationStudents",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeenByParent",
                table: "ViolationStudents");

            migrationBuilder.AddColumn<bool>(
                name: "SeenByParent",
                table: "Violations",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
