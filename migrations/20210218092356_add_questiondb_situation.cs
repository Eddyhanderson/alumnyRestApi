using Microsoft.EntityFrameworkCore.Migrations;

namespace alumni.Migrations
{
    public partial class add_questiondb_situation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Resolved",
                table: "Questions");

            migrationBuilder.AddColumn<string>(
                name: "Situation",
                table: "Questions",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Situation",
                table: "Questions");

            migrationBuilder.AddColumn<bool>(
                name: "Resolved",
                table: "Questions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
