using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace alumni.Migrations
{
    public partial class add_articledb_fields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Articles");

            migrationBuilder.AddColumn<string>(
                name: "Delta",
                table: "Articles",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Draft",
                table: "Articles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastChange",
                table: "Articles",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Situation",
                table: "Articles",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TeacherId",
                table: "Articles",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_TeacherId",
                table: "Articles",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Teachers_TeacherId",
                table: "Articles",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Teachers_TeacherId",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_TeacherId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Delta",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Draft",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "LastChange",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Situation",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Articles");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
