using Microsoft.EntityFrameworkCore.Migrations;

namespace alumni.Migrations
{
    public partial class studantdb_academy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Studants_Courses_CourseId",
                table: "Studants");

            migrationBuilder.AlterColumn<string>(
                name: "CourseId",
                table: "Studants",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "AcademicLevelId",
                table: "Studants",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AcademyId",
                table: "Studants",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Studants_AcademicLevelId",
                table: "Studants",
                column: "AcademicLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Studants_AcademyId",
                table: "Studants",
                column: "AcademyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Studants_AcademicLevels_AcademicLevelId",
                table: "Studants",
                column: "AcademicLevelId",
                principalTable: "AcademicLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Studants_Academies_AcademyId",
                table: "Studants",
                column: "AcademyId",
                principalTable: "Academies",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Studants_Courses_CourseId",
                table: "Studants",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Studants_AcademicLevels_AcademicLevelId",
                table: "Studants");

            migrationBuilder.DropForeignKey(
                name: "FK_Studants_Academies_AcademyId",
                table: "Studants");

            migrationBuilder.DropForeignKey(
                name: "FK_Studants_Courses_CourseId",
                table: "Studants");

            migrationBuilder.DropIndex(
                name: "IX_Studants_AcademicLevelId",
                table: "Studants");

            migrationBuilder.DropIndex(
                name: "IX_Studants_AcademyId",
                table: "Studants");

            migrationBuilder.DropColumn(
                name: "AcademicLevelId",
                table: "Studants");

            migrationBuilder.DropColumn(
                name: "AcademyId",
                table: "Studants");

            migrationBuilder.AlterColumn<string>(
                name: "CourseId",
                table: "Studants",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Studants_Courses_CourseId",
                table: "Studants",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
