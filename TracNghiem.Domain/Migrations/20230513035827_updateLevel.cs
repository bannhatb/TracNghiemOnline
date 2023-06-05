using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TracNghiem.Domain.Migrations
{
    /// <inheritdoc />
    public partial class updateLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Levels_LevelId",
                table: "Exams");

            migrationBuilder.DropIndex(
                name: "IX_Exams_LevelId",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "LevelId",
                table: "Exams");

            migrationBuilder.AddColumn<int>(
                name: "LevelId",
                table: "Questions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_LevelId",
                table: "Questions",
                column: "LevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Levels_LevelId",
                table: "Questions",
                column: "LevelId",
                principalTable: "Levels",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Levels_LevelId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_LevelId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "LevelId",
                table: "Questions");

            migrationBuilder.AddColumn<int>(
                name: "LevelId",
                table: "Exams",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exams_LevelId",
                table: "Exams",
                column: "LevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Levels_LevelId",
                table: "Exams",
                column: "LevelId",
                principalTable: "Levels",
                principalColumn: "Id");
        }
    }
}
