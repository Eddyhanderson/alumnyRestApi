using Microsoft.EntityFrameworkCore.Migrations;

namespace alumni.Migrations
{
    public partial class add_lessondb_topic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_DisciplineTopics_DiscpilineTopicId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_DiscpilineTopicId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "DiscpilineTopicId",
                table: "Lessons");

            migrationBuilder.AddColumn<string>(
                name: "PostId",
                table: "Topics",
                nullable: false,
                defaultValue: "1");

            migrationBuilder.AddColumn<string>(
                name: "TopicId",
                table: "Lessons",
                nullable: false,
                defaultValue: "1");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_PostId",
                table: "Topics",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_TopicId",
                table: "Lessons",
                column: "TopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Topics_TopicId",
                table: "Lessons",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Topics_Posts_PostId",
                table: "Topics",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Topics_TopicId",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_Topics_Posts_PostId",
                table: "Topics");

            migrationBuilder.DropIndex(
                name: "IX_Topics_PostId",
                table: "Topics");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_TopicId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "TopicId",
                table: "Lessons");

            migrationBuilder.AddColumn<string>(
                name: "DiscpilineTopicId",
                table: "Lessons",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_DiscpilineTopicId",
                table: "Lessons",
                column: "DiscpilineTopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_DisciplineTopics_DiscpilineTopicId",
                table: "Lessons",
                column: "DiscpilineTopicId",
                principalTable: "DisciplineTopics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
