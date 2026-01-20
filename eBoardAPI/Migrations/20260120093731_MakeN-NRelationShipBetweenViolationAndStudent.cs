using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eBoardAPI.Migrations
{
    /// <inheritdoc />
    public partial class MakeNNRelationShipBetweenViolationAndStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Violations_Students_StudentId",
                table: "Violations");

            migrationBuilder.DropIndex(
                name: "IX_Violations_StudentId",
                table: "Violations");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Violations");

            migrationBuilder.CreateTable(
                name: "ViolationStudents",
                columns: table => new
                {
                    ViolationId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViolationStudents", x => new { x.ViolationId, x.StudentId });
                    table.ForeignKey(
                        name: "FK_ViolationStudents_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ViolationStudents_Violations_ViolationId",
                        column: x => x.ViolationId,
                        principalTable: "Violations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ViolationStudents_StudentId",
                table: "ViolationStudents",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ViolationStudents");

            migrationBuilder.AddColumn<Guid>(
                name: "StudentId",
                table: "Violations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Violations_StudentId",
                table: "Violations",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Violations_Students_StudentId",
                table: "Violations",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
