using Microsoft.EntityFrameworkCore.Migrations;

namespace alumni.Migrations
{
    public partial class change_certificatedb_2022_03_02_00_47 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Certificate_CertificateId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_CertificateId",
                table: "Subscriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Certificate",
                table: "Certificate");

            migrationBuilder.DropColumn(
                name: "CertificateId",
                table: "Subscriptions");

            migrationBuilder.RenameTable(
                name: "Certificate",
                newName: "Certificates");

            migrationBuilder.AddColumn<string>(
                name: "AssignmentSchool",
                table: "Certificates",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MaxScore",
                table: "Certificates",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QualitativeResult",
                table: "Certificates",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubscriptionId",
                table: "Certificates",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Certificates",
                table: "Certificates",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_SubscriptionId",
                table: "Certificates",
                column: "SubscriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_Subscriptions_SubscriptionId",
                table: "Certificates",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_Subscriptions_SubscriptionId",
                table: "Certificates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Certificates",
                table: "Certificates");

            migrationBuilder.DropIndex(
                name: "IX_Certificates_SubscriptionId",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "AssignmentSchool",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "MaxScore",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "QualitativeResult",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                table: "Certificates");

            migrationBuilder.RenameTable(
                name: "Certificates",
                newName: "Certificate");

            migrationBuilder.AddColumn<string>(
                name: "CertificateId",
                table: "Subscriptions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Certificate",
                table: "Certificate",
                column: "Id");

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
    }
}
