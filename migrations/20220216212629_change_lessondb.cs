using Microsoft.EntityFrameworkCore.Migrations;

namespace alumni.Migrations
{
    public partial class change_lessondb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_TeacherPlaces_TeacherPlaceId",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Topics_TopicId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_TeacherPlaceId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_TopicId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "BackgroundPhotoPath",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "Public",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "TeacherPlaceId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "TopicId",
                table: "Lessons");

            migrationBuilder.AddColumn<string>(
                name: "ModuleId",
                table: "Lessons",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "Lessons",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_ModuleId",
                table: "Lessons",
                column: "ModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Modules_ModuleId",
                table: "Lessons",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Modules_ModuleId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_ModuleId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "ModuleId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "Picture",
                table: "Lessons");

            migrationBuilder.AddColumn<string>(
                name: "BackgroundPhotoPath",
                table: "Lessons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Public",
                table: "Lessons",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TeacherPlaceId",
                table: "Lessons",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TopicId",
                table: "Lessons",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_TeacherPlaceId",
                table: "Lessons",
                column: "TeacherPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_TopicId",
                table: "Lessons",
                column: "TopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_TeacherPlaces_TeacherPlaceId",
                table: "Lessons",
                column: "TeacherPlaceId",
                principalTable: "TeacherPlaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Topics_TopicId",
                table: "Lessons",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
