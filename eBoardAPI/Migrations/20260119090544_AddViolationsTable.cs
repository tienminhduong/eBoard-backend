using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eBoardAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddViolationsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FundIncomes_ClassFunds_ClassFundId",
                table: "FundIncomes");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClassFundId",
                table: "FundIncomes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Violations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    InChargeTeacherName = table.Column<string>(type: "text", nullable: false),
                    ViolateDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ViolationType = table.Column<string>(type: "text", nullable: false),
                    ViolationLevel = table.Column<int>(type: "integer", nullable: false),
                    ViolationInfo = table.Column<string>(type: "text", nullable: false),
                    Penalty = table.Column<string>(type: "text", nullable: false),
                    SeenByParent = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Violations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Violations_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Violations_StudentId",
                table: "Violations",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_FundIncomes_ClassFunds_ClassFundId",
                table: "FundIncomes",
                column: "ClassFundId",
                principalTable: "ClassFunds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FundIncomes_ClassFunds_ClassFundId",
                table: "FundIncomes");

            migrationBuilder.DropTable(
                name: "Violations");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClassFundId",
                table: "FundIncomes",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_FundIncomes_ClassFunds_ClassFundId",
                table: "FundIncomes",
                column: "ClassFundId",
                principalTable: "ClassFunds",
                principalColumn: "Id");
        }
    }
}
