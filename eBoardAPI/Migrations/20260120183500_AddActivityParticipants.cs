using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eBoardAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddActivityParticipants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityParticipants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ActivityId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParentPhoneNumber = table.Column<string>(type: "text", nullable: false),
                    TeacherComments = table.Column<string>(type: "text", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityParticipants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityParticipants_ExtracurricularActivities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "ExtracurricularActivities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityParticipants_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityParticipants_ActivityId",
                table: "ActivityParticipants",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityParticipants_StudentId",
                table: "ActivityParticipants",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityParticipants");
        }
    }
}
