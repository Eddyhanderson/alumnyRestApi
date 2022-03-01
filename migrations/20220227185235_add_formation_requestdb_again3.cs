using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace alumni.Migrations
{
    public partial class add_formation_requestdb_again3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Price",
                table: "Formations",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateTable(
                name: "FormationRequests",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    FormationId = table.Column<string>(nullable: false),
                    StudantId = table.Column<string>(nullable: false),
                    StudantMessage = table.Column<string>(nullable: false),
                    TeacherMessage = table.Column<string>(nullable: false),
                    State = table.Column<string>(nullable: false),
                    CreateAt = table.Column<DateTime>(nullable: false),
                    StateDate = table.Column<DateTime>(nullable: false),
                    Situation = table.Column<string>(nullable: false)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormationRequests");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Formations",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(float));
        }
    }
}
