using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace alumni.Migrations
{
    public partial class add_formation_requestdb_again : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FormationRequests",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreateAt = table.Column<DateTime>(nullable: false),
                    StateDate = table.Column<DateTime>(nullable: false),
                    FormationId = table.Column<string>(nullable: false),
                    StudantId = table.Column<string>(nullable: false),
                    Situation = table.Column<string>(nullable: true),
                    StudantMessage = table.Column<string>(nullable: true),
                    TeacherMessage = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormationRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormationRequests_Formations_FormationId",
                        column: x => x.FormationId,
                        principalTable: "Formations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_FormationRequests_Studants_StudantId",
                        column: x => x.StudantId,
                        principalTable: "Studants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormationRequests_FormationId",
                table: "FormationRequests",
                column: "FormationId");

            migrationBuilder.CreateIndex(
                name: "IX_FormationRequests_StudantId",
                table: "FormationRequests",
                column: "StudantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormationRequests");
        }
    }
}
