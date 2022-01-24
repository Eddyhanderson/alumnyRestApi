using Microsoft.EntityFrameworkCore.Migrations;

namespace alumni.Migrations
{
    public partial class add_questiondb_studant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Situation",
                table: "Questions",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudantId",
                table: "Questions",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_StudantId",
                table: "Questions",
                column: "StudantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Studants_StudantId",
                table: "Questions",
                column: "StudantId",
                principalTable: "Studants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Studants_StudantId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_StudantId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "StudantId",
                table: "Questions");

            migrationBuilder.AlterColumn<string>(
                name: "Situation",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
