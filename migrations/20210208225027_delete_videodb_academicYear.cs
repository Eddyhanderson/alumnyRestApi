using Microsoft.EntityFrameworkCore.Migrations;

namespace alumni.Migrations
{
    public partial class delete_videodb_academicYear : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcedmicYear",
                table: "TeacherPlaces");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AcedmicYear",
                table: "TeacherPlaces",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
