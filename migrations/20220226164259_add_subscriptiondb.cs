using Microsoft.EntityFrameworkCore.Migrations;

namespace alumni.Migrations
{
    public partial class add_subscriptiondb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    StudantId = table.Column<string>(nullable: true),
                    FormationEventId = table.Column<string>(nullable: true),
                    Situation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_FormationEvents_FormationEventId",
                        column: x => x.FormationEventId,
                        principalTable: "FormationEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Studants_StudantId",
                        column: x => x.StudantId,
                        principalTable: "Studants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_FormationEventId",
                table: "Subscriptions",
                column: "FormationEventId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_StudantId",
                table: "Subscriptions",
                column: "StudantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subscriptions");
        }
    }
}
