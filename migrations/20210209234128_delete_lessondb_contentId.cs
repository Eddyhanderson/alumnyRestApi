using Microsoft.EntityFrameworkCore.Migrations;

namespace alumni.Migrations
{
    public partial class delete_lessondb_contentId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentId",
                table: "Lessons");

            migrationBuilder.AddColumn<string>(
                name: "ArticleId",
                table: "Lessons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoId",
                table: "Lessons",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_ArticleId",
                table: "Lessons",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_VideoId",
                table: "Lessons",
                column: "VideoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Articles_ArticleId",
                table: "Lessons",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Videos_VideoId",
                table: "Lessons",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Articles_ArticleId",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Videos_VideoId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_ArticleId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_VideoId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "ArticleId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "VideoId",
                table: "Lessons");

            migrationBuilder.AddColumn<string>(
                name: "ContentId",
                table: "Lessons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
