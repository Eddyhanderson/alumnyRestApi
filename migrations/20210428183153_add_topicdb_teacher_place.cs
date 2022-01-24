using Microsoft.EntityFrameworkCore.Migrations;

namespace alumni.Migrations
{
    public partial class add_topicdb_teacher_place : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TeacherPlaceId",
                table: "Topics",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Topics_TeacherPlaceId",
                table: "Topics",
                column: "TeacherPlaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Topics_TeacherPlaces_TeacherPlaceId",
                table: "Topics",
                column: "TeacherPlaceId",
                principalTable: "TeacherPlaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Topics_TeacherPlaces_TeacherPlaceId",
                table: "Topics");

            migrationBuilder.DropIndex(
                name: "IX_Topics_TeacherPlaceId",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "TeacherPlaceId",
                table: "Topics");
        }
    }
}
