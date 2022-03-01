using Microsoft.EntityFrameworkCore.Migrations;

namespace alumni.Migrations
{
    public partial class add_school_identitydb_school_code : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SchoolCode",
                table: "SchoolIdentities",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SchoolCode",
                table: "SchoolIdentities");
        }
    }
}
