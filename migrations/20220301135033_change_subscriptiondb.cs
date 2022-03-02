using Microsoft.EntityFrameworkCore.Migrations;

namespace alumni.Migrations
{
    public partial class change_subscriptiondb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CertificateId",
                table: "Subscriptions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Subscriptions",
                defaultValue: "Learning",
                nullable: false);

            migrationBuilder.CreateTable(
                name: "Certificate",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AssessmentMethod = table.Column<string>(nullable: true),
                    AssessmentScore = table.Column<string>(nullable: true),
                    PathCertificate = table.Column<string>(nullable: true),
                    Observation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificate", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_CertificateId",
                table: "Subscriptions",
                column: "CertificateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Certificate_CertificateId",
                table: "Subscriptions",
                column: "CertificateId",
                principalTable: "Certificate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Certificate_CertificateId",
                table: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Certificate");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_CertificateId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "CertificateId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Subscriptions");
        }
    }
}
