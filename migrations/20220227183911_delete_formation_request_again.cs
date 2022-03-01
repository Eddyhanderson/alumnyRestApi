using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace alumni.Migrations
{
    public partial class delete_formation_request_again : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormationRequests");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_FormationRequests_FormationId",
                table: "FormationRequests",
                column: "FormationId");

            migrationBuilder.CreateIndex(
                name: "IX_FormationRequests_StudantId",
                table: "FormationRequests",
                column: "StudantId");
        }
    }
}
