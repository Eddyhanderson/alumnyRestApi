using Microsoft.EntityFrameworkCore.Migrations;

namespace alumni.Migrations
{
    public partial class remove_schooldb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolCourses_Schools_SchoolId",
                table: "SchoolCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_Schools_SchoolIdentities_SchoolIdentityId",
                table: "Schools");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherPlaces_Schools_SchoolId",
                table: "TeacherPlaces");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherSchools_Schools_SchoolId",
                table: "TeacherSchools");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Schools",
                table: "Schools");

            migrationBuilder.RenameTable(
                name: "Schools",
                newName: "School");

            migrationBuilder.RenameIndex(
                name: "IX_Schools_SchoolIdentityId",
                table: "School",
                newName: "IX_School_SchoolIdentityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_School",
                table: "School",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_School_SchoolIdentities_SchoolIdentityId",
                table: "School",
                column: "SchoolIdentityId",
                principalTable: "SchoolIdentities",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolCourses_School_SchoolId",
                table: "SchoolCourses",
                column: "SchoolId",
                principalTable: "School",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherPlaces_School_SchoolId",
                table: "TeacherPlaces",
                column: "SchoolId",
                principalTable: "School",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherSchools_School_SchoolId",
                table: "TeacherSchools",
                column: "SchoolId",
                principalTable: "School",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_School_SchoolIdentities_SchoolIdentityId",
                table: "School");

            migrationBuilder.DropForeignKey(
                name: "FK_SchoolCourses_School_SchoolId",
                table: "SchoolCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherPlaces_School_SchoolId",
                table: "TeacherPlaces");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherSchools_School_SchoolId",
                table: "TeacherSchools");

            migrationBuilder.DropPrimaryKey(
                name: "PK_School",
                table: "School");

            migrationBuilder.RenameTable(
                name: "School",
                newName: "Schools");

            migrationBuilder.RenameIndex(
                name: "IX_School_SchoolIdentityId",
                table: "Schools",
                newName: "IX_Schools_SchoolIdentityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Schools",
                table: "Schools",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolCourses_Schools_SchoolId",
                table: "SchoolCourses",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Schools_SchoolIdentities_SchoolIdentityId",
                table: "Schools",
                column: "SchoolIdentityId",
                principalTable: "SchoolIdentities",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherPlaces_Schools_SchoolId",
                table: "TeacherPlaces",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherSchools_Schools_SchoolId",
                table: "TeacherSchools",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
