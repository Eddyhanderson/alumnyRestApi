using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace alumni.Migrations
{
    public partial class change_formationdb_picture_and_add_formationEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "Formations",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Published",
                table: "Formations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "FormationEvents",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Start = table.Column<DateTime>(nullable: false),
                    End = table.Column<DateTime>(nullable: false),
                    StudantLimit = table.Column<int>(nullable: false),
                    State = table.Column<string>(nullable: false),
                    Situation = table.Column<string>(nullable: false),
                    FormationId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormationEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormationEvents_Formations_FormationId",
                        column: x => x.FormationId,
                        principalTable: "Formations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormationEvents_FormationId",
                table: "FormationEvents",
                column: "FormationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormationEvents");

            migrationBuilder.DropColumn(
                name: "Picture",
                table: "Formations");

            migrationBuilder.DropColumn(
                name: "Published",
                table: "Formations");
        }
    }
}
