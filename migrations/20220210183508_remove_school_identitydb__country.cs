using Microsoft.EntityFrameworkCore.Migrations;

namespace alumni.Migrations
{
    public partial class remove_school_identitydb__country : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "SchoolIdentities");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "SchoolIdentities",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
