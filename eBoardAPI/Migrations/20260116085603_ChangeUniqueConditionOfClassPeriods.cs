using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eBoardAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUniqueConditionOfClassPeriods : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ClassPeriods_ClassId_PeriodNumber",
                table: "ClassPeriods");

            migrationBuilder.CreateIndex(
                name: "IX_ClassPeriods_ClassId",
                table: "ClassPeriods",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassPeriods_IsMorningPeriod_PeriodNumber_DayOfWeek_ClassId",
                table: "ClassPeriods",
                columns: new[] { "IsMorningPeriod", "PeriodNumber", "DayOfWeek", "ClassId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ClassPeriods_ClassId",
                table: "ClassPeriods");

            migrationBuilder.DropIndex(
                name: "IX_ClassPeriods_IsMorningPeriod_PeriodNumber_DayOfWeek_ClassId",
                table: "ClassPeriods");

            migrationBuilder.CreateIndex(
                name: "IX_ClassPeriods_ClassId_PeriodNumber",
                table: "ClassPeriods",
                columns: new[] { "ClassId", "PeriodNumber" },
                unique: true);
        }
    }
}
