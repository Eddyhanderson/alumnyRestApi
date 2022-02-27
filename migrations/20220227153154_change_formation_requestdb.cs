using Microsoft.EntityFrameworkCore.Migrations;

namespace alumni.Migrations
{
    public partial class change_formation_requestdb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormationRequests_Formations_FormationId",
                table: "FormationRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_FormationRequests_Studants_StudantId",
                table: "FormationRequests");

            migrationBuilder.AlterColumn<string>(
                name: "StudantId",
                table: "FormationRequests",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FormationId",
                table: "FormationRequests",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FormationRequests_Formations_FormationId",
                table: "FormationRequests",
                column: "FormationId",
                principalTable: "Formations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FormationRequests_Studants_StudantId",
                table: "FormationRequests",
                column: "StudantId",
                principalTable: "Studants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddColumn<string>(
            name: "StudantMessage",
            table: "FormationRequests",
            nullable: true);

            migrationBuilder.AddColumn<string>(
            name: "TeacherMessage",
            table: "FormationRequests",
            nullable: true);

            migrationBuilder.AddColumn<string>(
            name: "State",
            table: "FormationRequests",
            nullable: false,
            defaultValue: '1');
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormationRequests_Formations_FormationId",
                table: "FormationRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_FormationRequests_Studants_StudantId",
                table: "FormationRequests");

            migrationBuilder.AlterColumn<string>(
                name: "StudantId",
                table: "FormationRequests",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "FormationId",
                table: "FormationRequests",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_FormationRequests_Formations_FormationId",
                table: "FormationRequests",
                column: "FormationId",
                principalTable: "Formations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FormationRequests_Studants_StudantId",
                table: "FormationRequests",
                column: "StudantId",
                principalTable: "Studants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
