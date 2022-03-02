using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace alumni.Migrations
{
    public partial class change_certificatedb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EmitedAt",
                table: "Certificate",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmitedAt",
                table: "Certificate");
        }
    }
}
