using Microsoft.EntityFrameworkCore.Migrations;

namespace alumni.Migrations
{
    public partial class change_articledb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Teachers_TeacherId",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_TeacherId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Articles");

            migrationBuilder.AddColumn<string>(
                name: "ModuleId",
                table: "Articles",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ModuleId",
                table: "Articles",
                column: "ModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Modules_ModuleId",
                table: "Articles",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Modules_ModuleId",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_ModuleId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "ModuleId",
                table: "Articles");

            migrationBuilder.AddColumn<string>(
                name: "TeacherId",
                table: "Articles",
                type: "nvarchar(450)",
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
    }
}
