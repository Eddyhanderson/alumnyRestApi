using Microsoft.EntityFrameworkCore.Migrations;

namespace alumni.Migrations
{
    public partial class change_school_identitydb_short_name_to_acronym : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortName",
                table: "SchoolIdentities");

            migrationBuilder.AddColumn<string>(
                name: "Acronym",
                table: "SchoolIdentities",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Acronym",
                table: "SchoolIdentities");

            migrationBuilder.AddColumn<string>(
                name: "ShortName",
                table: "SchoolIdentities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
