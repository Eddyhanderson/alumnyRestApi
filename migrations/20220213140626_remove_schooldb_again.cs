using Microsoft.EntityFrameworkCore.Migrations;

namespace alumni.Migrations
{
    public partial class remove_schooldb_again : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherPlaces_School_SchoolId",
                table: "TeacherPlaces");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherSchools_School_SchoolId",
                table: "TeacherSchools");

            migrationBuilder.DropTable(
                name: "SchoolCourses");

            migrationBuilder.DropTable(
                name: "School");

            migrationBuilder.DropTable(
                name: "SchoolIdentities");                    
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherPlaces_SchoolDeprecated_SchoolId",
                table: "TeacherPlaces");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherSchools_SchoolDeprecated_SchoolId",
                table: "TeacherSchools");

            migrationBuilder.DropTable(
                name: "SchoolDeprecated");

            migrationBuilder.CreateTable(
                name: "SchoolIdentities",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Acronym = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nif = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SchoolCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolIdentities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolIdentities_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "School",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SchoolIdentityId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_School", x => x.Id);
                    table.ForeignKey(
                        name: "FK_School_SchoolIdentities_SchoolIdentityId",
                        column: x => x.SchoolIdentityId,
                        principalTable: "SchoolIdentities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolCourses",
                columns: table => new
                {
                    CourseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SchoolId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Situation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolCourses", x => x.CourseId);
                    table.ForeignKey(
                        name: "FK_SchoolCourses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolCourses_School_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "School",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_School_SchoolIdentityId",
                table: "School",
                column: "SchoolIdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolCourses_SchoolId",
                table: "SchoolCourses",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolIdentities_UserId",
                table: "SchoolIdentities",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherPlaces_School_SchoolId",
                table: "TeacherPlaces",
                column: "SchoolId",
                principalTable: "School",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherSchools_School_SchoolId",
                table: "TeacherSchools",
                column: "SchoolId",
                principalTable: "School",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
