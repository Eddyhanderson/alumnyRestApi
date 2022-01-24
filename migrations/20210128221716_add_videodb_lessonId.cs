using Microsoft.EntityFrameworkCore.Migrations;

namespace alumni.Migrations
{
    public partial class add_videodb_lessonId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Videos_VideoId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_VideoId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "VideoId",
                table: "Lessons");

            migrationBuilder.AddColumn<string>(
                name: "LessonId",
                table: "Videos",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Videos_LessonId",
                table: "Videos",
                column: "LessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Lessons_LessonId",
                table: "Videos",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Lessons_LessonId",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Videos_LessonId",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "Videos");

            migrationBuilder.AddColumn<string>(
                name: "VideoId",
                table: "Lessons",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_VideoId",
                table: "Lessons",
                column: "VideoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Videos_VideoId",
                table: "Lessons",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
