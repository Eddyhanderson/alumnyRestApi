using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace alumni.Migrations
{
    public partial class insert_some_managment_data_model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Managers_Schools_SchoolId",
                table: "Managers");

            migrationBuilder.DropForeignKey(
                name: "FK_Schools_BadgeInformations_BadgeInformationId",
                table: "Schools");

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

            migrationBuilder.DropIndex(
                name: "IX_Studants_CourseId",
                table: "Studants");

            migrationBuilder.DropIndex(
                name: "IX_Schools_BadgeInformationId",
                table: "Schools");

            migrationBuilder.DropIndex(
                name: "IX_Managers_SchoolId",
                table: "Managers");

            migrationBuilder.DropColumn(
                name: "AcademicLevelId",
                table: "Studants");

            migrationBuilder.DropColumn(
                name: "AcademyId",
                table: "Studants");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Studants");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "BadgeInformationId",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "Entrusted",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "Nif",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "ShortName",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "DateSituation",
                table: "Managers");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "Managers");

            migrationBuilder.DropColumn(
                name: "Situation",
                table: "Managers");

            migrationBuilder.DropColumn(
                name: "AboutUser",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Birth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreationAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Genre",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Studants",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Studants",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OrganId",
                table: "Studants",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SchoolIdentityId",
                table: "Schools",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Organ",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Managers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Managers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateSituation",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "SchoolIdentities",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    ShortName = table.Column<string>(nullable: false),
                    Country = table.Column<string>(nullable: true),
                    Nif = table.Column<string>(nullable: false),
                    Adress = table.Column<string>(nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_Studants_OrganId",
                table: "Studants",
                column: "OrganId");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_SchoolIdentityId",
                table: "Schools",
                column: "SchoolIdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_Organ_UserId",
                table: "Organ",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolIdentities_UserId",
                table: "SchoolIdentities",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Organ_AspNetUsers_UserId",
                table: "Organ",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schools_SchoolIdentities_SchoolIdentityId",
                table: "Schools",
                column: "SchoolIdentityId",
                principalTable: "SchoolIdentities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Studants_Organ_OrganId",
                table: "Studants",
                column: "OrganId",
                principalTable: "Organ",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organ_AspNetUsers_UserId",
                table: "Organ");

            migrationBuilder.DropForeignKey(
                name: "FK_Schools_SchoolIdentities_SchoolIdentityId",
                table: "Schools");

            migrationBuilder.DropForeignKey(
                name: "FK_Studants_Organ_OrganId",
                table: "Studants");

            migrationBuilder.DropTable(
                name: "SchoolIdentities");

            migrationBuilder.DropIndex(
                name: "IX_Studants_OrganId",
                table: "Studants");

            migrationBuilder.DropIndex(
                name: "IX_Schools_SchoolIdentityId",
                table: "Schools");

            migrationBuilder.DropIndex(
                name: "IX_Organ_UserId",
                table: "Organ");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Studants");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Studants");

            migrationBuilder.DropColumn(
                name: "OrganId",
                table: "Studants");

            migrationBuilder.DropColumn(
                name: "SchoolIdentityId",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Organ");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Managers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Managers");

            migrationBuilder.DropColumn(
                name: "DateSituation",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "AcademicLevelId",
                table: "Studants",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AcademyId",
                table: "Studants",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CourseId",
                table: "Studants",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Schools",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BadgeInformationId",
                table: "Schools",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Entrusted",
                table: "Schools",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Schools",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nif",
                table: "Schools",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShortName",
                table: "Schools",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateSituation",
                table: "Managers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "SchoolId",
                table: "Managers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Situation",
                table: "Managers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AboutUser",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Birth",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Genre",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Studants_AcademicLevelId",
                table: "Studants",
                column: "AcademicLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Studants_AcademyId",
                table: "Studants",
                column: "AcademyId");

            migrationBuilder.CreateIndex(
                name: "IX_Studants_CourseId",
                table: "Studants",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_BadgeInformationId",
                table: "Schools",
                column: "BadgeInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Managers_SchoolId",
                table: "Managers",
                column: "SchoolId");

            migrationBuilder.AddForeignKey(
                name: "FK_Managers_Schools_SchoolId",
                table: "Managers",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Schools_BadgeInformations_BadgeInformationId",
                table: "Schools",
                column: "BadgeInformationId",
                principalTable: "BadgeInformations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Studants_AcademicLevels_AcademicLevelId",
                table: "Studants",
                column: "AcademicLevelId",
                principalTable: "AcademicLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Studants_Academies_AcademyId",
                table: "Studants",
                column: "AcademyId",
                principalTable: "Academies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Studants_Courses_CourseId",
                table: "Studants",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
