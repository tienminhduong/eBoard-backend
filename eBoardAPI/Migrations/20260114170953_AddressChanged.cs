using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eBoardAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddressChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Districts_DistrictId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Provinces_ProvinceId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Wards_WardId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "Wards");

            migrationBuilder.DropTable(
                name: "Districts");

            migrationBuilder.DropTable(
                name: "Provinces");

            migrationBuilder.DropIndex(
                name: "IX_Students_DistrictId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_ProvinceId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_WardId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "ProvinceId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "WardId",
                table: "Students");

            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "Students",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Province",
                table: "Students",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Ward",
                table: "Students",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "District",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Province",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Ward",
                table: "Students");

            migrationBuilder.AddColumn<Guid>(
                name: "DistrictId",
                table: "Students",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProvinceId",
                table: "Students",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WardId",
                table: "Students",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Provinces",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provinces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProvinceId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Districts_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Wards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DistrictId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wards_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_DistrictId",
                table: "Students",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_ProvinceId",
                table: "Students",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_WardId",
                table: "Students",
                column: "WardId");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_ProvinceId",
                table: "Districts",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Wards_DistrictId",
                table: "Wards",
                column: "DistrictId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Districts_DistrictId",
                table: "Students",
                column: "DistrictId",
                principalTable: "Districts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Provinces_ProvinceId",
                table: "Students",
                column: "ProvinceId",
                principalTable: "Provinces",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Wards_WardId",
                table: "Students",
                column: "WardId",
                principalTable: "Wards",
                principalColumn: "Id");
        }
    }
}
