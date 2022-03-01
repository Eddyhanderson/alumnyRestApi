using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace alumni.Migrations
{
    public partial class new_organdb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.CreateTable(
                name: "Organ",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Code = table.Column<decimal>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Badget = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organ", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {           
            migrationBuilder.DropColumn(
                name: "RegisteredAt",
                table: "TeacherPlaceStudants");
        }
    }
}
