using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eBoardAPI.Migrations
{
    /// <inheritdoc />
    public partial class Activity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExtracurricularActivities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: false),
                    MaxParticipants = table.Column<int>(type: "integer", nullable: false),
                    InChargeTeacher = table.Column<string>(type: "text", nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Cost = table.Column<int>(type: "integer", nullable: false),
                    AssignDeadline = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtracurricularActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExtracurricularActivities_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivitySignIns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ActivityId = table.Column<Guid>(type: "uuid", nullable: false),
                    SignInTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivitySignIns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivitySignIns_ExtracurricularActivities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "ExtracurricularActivities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivitySignIns_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_ClassId",
                table: "Attendances",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_AbsentRequests_ClassId",
                table: "AbsentRequests",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_AbsentRequests_StudentId",
                table: "AbsentRequests",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivitySignIns_ActivityId",
                table: "ActivitySignIns",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivitySignIns_StudentId",
                table: "ActivitySignIns",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtracurricularActivities_ClassId",
                table: "ExtracurricularActivities",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbsentRequests_Classes_ClassId",
                table: "AbsentRequests",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AbsentRequests_Students_StudentId",
                table: "AbsentRequests",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Classes_ClassId",
                table: "Attendances",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Students_StudentId",
                table: "Attendances",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbsentRequests_Classes_ClassId",
                table: "AbsentRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_AbsentRequests_Students_StudentId",
                table: "AbsentRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Classes_ClassId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Students_StudentId",
                table: "Attendances");

            migrationBuilder.DropTable(
                name: "ActivitySignIns");

            migrationBuilder.DropTable(
                name: "ExtracurricularActivities");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_ClassId",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_AbsentRequests_ClassId",
                table: "AbsentRequests");

            migrationBuilder.DropIndex(
                name: "IX_AbsentRequests_StudentId",
                table: "AbsentRequests");
        }
    }
}
