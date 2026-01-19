using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eBoardAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeperateMorningAndAfternoonPeriod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ScheduleSettingDetails",
                table: "ScheduleSettingDetails");

            migrationBuilder.AddColumn<bool>(
                name: "IsMorningPeriod",
                table: "ScheduleSettingDetails",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMorningPeriod",
                table: "ClassPeriods",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScheduleSettingDetails",
                table: "ScheduleSettingDetails",
                columns: new[] { "PeriodNumber", "ScheduleSettingId", "IsMorningPeriod" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ScheduleSettingDetails",
                table: "ScheduleSettingDetails");

            migrationBuilder.DropColumn(
                name: "IsMorningPeriod",
                table: "ScheduleSettingDetails");

            migrationBuilder.DropColumn(
                name: "IsMorningPeriod",
                table: "ClassPeriods");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScheduleSettingDetails",
                table: "ScheduleSettingDetails",
                columns: new[] { "PeriodNumber", "ScheduleSettingId" });
        }
    }
}
