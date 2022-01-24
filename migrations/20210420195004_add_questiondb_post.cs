using Microsoft.EntityFrameworkCore.Migrations;

namespace alumni.Migrations
{
    public partial class add_questiondb_post : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PostId",
                table: "Answers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_PostId",
                table: "Answers",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Posts_PostId",
                table: "Answers",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Posts_PostId",
                table: "Answers");

            migrationBuilder.DropIndex(
                name: "IX_Answers_PostId",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Answers");
        }
    }
}
