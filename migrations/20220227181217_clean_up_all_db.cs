using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace alumni.Migrations
{
    public partial class clean_up_all_db : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "FormationRequests");

            migrationBuilder.DropTable(
                name: "TeacherPlaceMaterials");

            migrationBuilder.DropTable(
                name: "TeacherPlaceMessages");

            migrationBuilder.DropTable(
                name: "TeacherPlaceStudants");

            migrationBuilder.DropTable(
                name: "TeacherSchools");

            migrationBuilder.DropTable(
                name: "Topics");

            migrationBuilder.DropTable(
                name: "DisciplineTopics");

            migrationBuilder.DropTable(
                name: "TeacherPlaces");

            migrationBuilder.DropTable(
                name: "Disciplines");           

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "AcademicLevels");

            migrationBuilder.DropTable(
                name: "Academies");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "BadgeInformations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AcademicLevels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Admins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BadgeInformations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateSituation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProfilePhotoPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Situation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BadgeInformations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BadgeInformations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormationRequests",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FormationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Situation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StudantId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormationRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormationRequests_Formations_FormationId",
                        column: x => x.FormationId,
                        principalTable: "Formations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormationRequests_Studants_StudantId",
                        column: x => x.StudantId,
                        principalTable: "Studants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolDeprecated",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SchoolIdentityId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolDeprecated", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeacherPlaceMessages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TeacherPlaceId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherPlaceMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherPlaceMessages_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Academies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BadgeInformationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Academies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Academies_BadgeInformations_BadgeInformationId",
                        column: x => x.BadgeInformationId,
                        principalTable: "BadgeInformations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BadgeInformationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_BadgeInformations_BadgeInformationId",
                        column: x => x.BadgeInformationId,
                        principalTable: "BadgeInformations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Disciplines",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BadgeInformationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disciplines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Disciplines_BadgeInformations_BadgeInformationId",
                        column: x => x.BadgeInformationId,
                        principalTable: "BadgeInformations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DisciplineTopics",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BadgeInformationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisciplineTopics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DisciplineTopics_BadgeInformations_BadgeInformationId",
                        column: x => x.BadgeInformationId,
                        principalTable: "BadgeInformations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AcademicLevelId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AcademyId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CourseId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TeacherCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teachers_AcademicLevels_AcademicLevelId",
                        column: x => x.AcademicLevelId,
                        principalTable: "AcademicLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Teachers_Academies_AcademyId",
                        column: x => x.AcademyId,
                        principalTable: "Academies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Teachers_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Teachers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherPlaces",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisciplineId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Opened = table.Column<bool>(type: "bit", nullable: false),
                    ProfilePhotoPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SchoolId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Situation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeacherId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TeacherPlaceCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherPlaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherPlaces_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherPlaces_Disciplines_DisciplineId",
                        column: x => x.DisciplineId,
                        principalTable: "Disciplines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherPlaces_SchoolDeprecated_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolDeprecated",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherPlaces_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherSchools",
                columns: table => new
                {
                    TeacherId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreationAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateSituation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SchoolId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Situation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherSchools", x => x.TeacherId);
                    table.ForeignKey(
                        name: "FK_TeacherSchools_SchoolDeprecated_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolDeprecated",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherSchools_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherPlaceMaterials",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaterialPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TeacherPlaceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherPlaceMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherPlaceMaterials_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherPlaceMaterials_TeacherPlaces_TeacherPlaceId",
                        column: x => x.TeacherPlaceId,
                        principalTable: "TeacherPlaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherPlaceStudants",
                columns: table => new
                {
                    TeacherPlaceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateSituation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Months = table.Column<int>(type: "int", nullable: false),
                    RegisteredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Situation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudantId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherPlaceStudants", x => x.TeacherPlaceId);
                    table.ForeignKey(
                        name: "FK_TeacherPlaceStudants_Studants_StudantId",
                        column: x => x.StudantId,
                        principalTable: "Studants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherPlaceStudants_TeacherPlaces_TeacherPlaceId",
                        column: x => x.TeacherPlaceId,
                        principalTable: "TeacherPlaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisciplineTopicId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PhotoProfile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TeacherPlaceId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Topics_DisciplineTopics_DisciplineTopicId",
                        column: x => x.DisciplineTopicId,
                        principalTable: "DisciplineTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Topics_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Topics_TeacherPlaces_TeacherPlaceId",
                        column: x => x.TeacherPlaceId,
                        principalTable: "TeacherPlaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Academies_BadgeInformationId",
                table: "Academies",
                column: "BadgeInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Admins_UserId",
                table: "Admins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BadgeInformations_UserId",
                table: "BadgeInformations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_BadgeInformationId",
                table: "Courses",
                column: "BadgeInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Disciplines_BadgeInformationId",
                table: "Disciplines",
                column: "BadgeInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_DisciplineTopics_BadgeInformationId",
                table: "DisciplineTopics",
                column: "BadgeInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_FormationRequests_FormationId",
                table: "FormationRequests",
                column: "FormationId");

            migrationBuilder.CreateIndex(
                name: "IX_FormationRequests_StudantId",
                table: "FormationRequests",
                column: "StudantId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherPlaceMaterials_PostId",
                table: "TeacherPlaceMaterials",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherPlaceMaterials_TeacherPlaceId",
                table: "TeacherPlaceMaterials",
                column: "TeacherPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherPlaceMessages_PostId",
                table: "TeacherPlaceMessages",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherPlaces_CourseId",
                table: "TeacherPlaces",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherPlaces_DisciplineId",
                table: "TeacherPlaces",
                column: "DisciplineId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherPlaces_SchoolId",
                table: "TeacherPlaces",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherPlaces_TeacherId",
                table: "TeacherPlaces",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherPlaceStudants_StudantId",
                table: "TeacherPlaceStudants",
                column: "StudantId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_AcademicLevelId",
                table: "Teachers",
                column: "AcademicLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_AcademyId",
                table: "Teachers",
                column: "AcademyId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_CourseId",
                table: "Teachers",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_UserId",
                table: "Teachers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherSchools_SchoolId",
                table: "TeacherSchools",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_DisciplineTopicId",
                table: "Topics",
                column: "DisciplineTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_PostId",
                table: "Topics",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_TeacherPlaceId",
                table: "Topics",
                column: "TeacherPlaceId");
        }
    }
}
