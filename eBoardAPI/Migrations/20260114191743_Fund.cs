using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eBoardAPI.Migrations
{
    /// <inheritdoc />
    public partial class Fund : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClassFunds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassId = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrentBalance = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassFunds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FundExpenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassFundId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    ExpenseDate = table.Column<DateOnly>(type: "date", nullable: false),
                    SpenderName = table.Column<string>(type: "text", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: false),
                    InvoiceImgUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FundExpenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FundExpenses_ClassFunds_ClassFundId",
                        column: x => x.ClassFundId,
                        principalTable: "ClassFunds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FundIncomes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassFundId = table.Column<Guid>(type: "uuid", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: false),
                    ExpectedAmount = table.Column<int>(type: "integer", nullable: false),
                    CollectedAmount = table.Column<int>(type: "integer", nullable: false),
                    AmountPerStudent = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FundIncomes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FundIncomes_ClassFunds_ClassFundId",
                        column: x => x.ClassFundId,
                        principalTable: "ClassFunds",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FundIncomeDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FundIncomeId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContributedAmount = table.Column<int>(type: "integer", nullable: false),
                    ContributedInfo = table.Column<string>(type: "text", nullable: false),
                    ContributedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    ContributionStatus = table.Column<string>(type: "text", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FundIncomeDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FundIncomeDetails_FundIncomes_FundIncomeId",
                        column: x => x.FundIncomeId,
                        principalTable: "FundIncomes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FundIncomeDetails_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FundExpenses_ClassFundId",
                table: "FundExpenses",
                column: "ClassFundId");

            migrationBuilder.CreateIndex(
                name: "IX_FundIncomeDetails_FundIncomeId",
                table: "FundIncomeDetails",
                column: "FundIncomeId");

            migrationBuilder.CreateIndex(
                name: "IX_FundIncomeDetails_StudentId",
                table: "FundIncomeDetails",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_FundIncomes_ClassFundId",
                table: "FundIncomes",
                column: "ClassFundId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FundExpenses");

            migrationBuilder.DropTable(
                name: "FundIncomeDetails");

            migrationBuilder.DropTable(
                name: "FundIncomes");

            migrationBuilder.DropTable(
                name: "ClassFunds");
        }
    }
}
