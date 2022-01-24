using Microsoft.EntityFrameworkCore.Migrations;

namespace alumni.Migrations
{
    public partial class change_lessondb_videodb_articledb_identifiers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Lessons_LessonId",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Lessons_LessonId",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Videos_LessonId",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Articles_LessonId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "Articles");

            migrationBuilder.AddColumn<string>(
                name: "ContentId",
                table: "Lessons",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentId",
                table: "Lessons");

            migrationBuilder.AddColumn<string>(
                name: "LessonId",
                table: "Videos",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LessonId",
                table: "Articles",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Videos_LessonId",
                table: "Videos",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_LessonId",
                table: "Articles",
                column: "LessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Lessons_LessonId",
                table: "Articles",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Lessons_LessonId",
                table: "Videos",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
