using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eBoardAPI.Migrations
{
    /// <inheritdoc />
    public partial class AcademicYearForClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcademicYear",
                table: "Classes");

            migrationBuilder.AddColumn<int>(
                name: "AcademicEndYear",
                table: "Classes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AcademicStartYear",
                table: "Classes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcademicEndYear",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "AcademicStartYear",
                table: "Classes");

            migrationBuilder.AddColumn<string>(
                name: "AcademicYear",
                table: "Classes",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
