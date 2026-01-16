using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eBoardAPI.Migrations
{
    /// <inheritdoc />
    public partial class Schedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScheduleSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassId = table.Column<Guid>(type: "uuid", nullable: false),
                    MorningPeriodCount = table.Column<int>(type: "integer", nullable: false),
                    AfternoonPeriodCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduleSettings_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleSettingDetails",
                columns: table => new
                {
                    PeriodNumber = table.Column<int>(type: "integer", nullable: false),
                    ScheduleSettingId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleSettingDetails", x => new { x.PeriodNumber, x.ScheduleSettingId });
                    table.ForeignKey(
                        name: "FK_ScheduleSettingDetails_ScheduleSettings_ScheduleSettingId",
                        column: x => x.ScheduleSettingId,
                        principalTable: "ScheduleSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassPeriods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    DayOfWeek = table.Column<int>(type: "integer", nullable: false),
                    PeriodNumber = table.Column<int>(type: "integer", nullable: false),
                    TeacherName = table.Column<string>(type: "text", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassPeriods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassPeriods_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassPeriods_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassFunds_ClassId",
                table: "ClassFunds",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassPeriods_ClassId_PeriodNumber",
                table: "ClassPeriods",
                columns: new[] { "ClassId", "PeriodNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassPeriods_SubjectId",
                table: "ClassPeriods",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleSettingDetails_ScheduleSettingId",
                table: "ScheduleSettingDetails",
                column: "ScheduleSettingId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleSettings_ClassId",
                table: "ScheduleSettings",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassFunds_Classes_ClassId",
                table: "ClassFunds",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassFunds_Classes_ClassId",
                table: "ClassFunds");

            migrationBuilder.DropTable(
                name: "ClassPeriods");

            migrationBuilder.DropTable(
                name: "ScheduleSettingDetails");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "ScheduleSettings");

            migrationBuilder.DropIndex(
                name: "IX_ClassFunds_ClassId",
                table: "ClassFunds");
        }
    }
}
