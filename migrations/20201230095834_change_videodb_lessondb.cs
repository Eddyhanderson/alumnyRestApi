using Microsoft.EntityFrameworkCore.Migrations;

namespace alumni.Migrations
{
    public partial class change_videodb_lessondb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Resolution",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "BackgroundPhotoPath",
                table: "Lessons");

            migrationBuilder.AddColumn<bool>(
                name: "Converted",
                table: "Videos",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ShortVideoPath",
                table: "Videos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailPath",
                table: "Videos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoId",
                table: "Lessons",
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
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Videos_VideoId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_VideoId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "Converted",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "ShortVideoPath",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "ThumbnailPath",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "VideoId",
                table: "Lessons");

            migrationBuilder.AddColumn<string>(
                name: "LessonId",
                table: "Videos",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Resolution",
                table: "Videos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Size",
                table: "Videos",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "BackgroundPhotoPath",
                table: "Lessons",
                type: "nvarchar(max)",
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
                onDelete: ReferentialAction.Cascade);
        }
    }
}
