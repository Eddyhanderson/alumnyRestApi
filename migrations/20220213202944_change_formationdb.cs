using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace alumni.Migrations
{
    public partial class change_formationdb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreation",
                table: "Formations",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateSituation",
                table: "Formations",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Situation",
                table: "Formations",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreation",
                table: "Formations");

            migrationBuilder.DropColumn(
                name: "DateSituation",
                table: "Formations");

            migrationBuilder.DropColumn(
                name: "Situation",
                table: "Formations");
        }
    }
}
