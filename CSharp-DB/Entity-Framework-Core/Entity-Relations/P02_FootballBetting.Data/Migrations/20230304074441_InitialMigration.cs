using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P02_FootballBetting.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Teams_TeamId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Teams_TeamId1",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_TeamId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_TeamId1",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "TeamId1",
                table: "Games");

            migrationBuilder.CreateIndex(
                name: "IX_Games_AwayTeamId",
                table: "Games",
                column: "AwayTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_HomeTeamId",
                table: "Games",
                column: "HomeTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Teams_AwayTeamId",
                table: "Games",
                column: "AwayTeamId",
                principalTable: "Teams",
                principalColumn: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Teams_HomeTeamId",
                table: "Games",
                column: "HomeTeamId",
                principalTable: "Teams",
                principalColumn: "TeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Teams_AwayTeamId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Teams_HomeTeamId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_AwayTeamId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_HomeTeamId",
                table: "Games");

            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "Games",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamId1",
                table: "Games",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_TeamId",
                table: "Games",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_TeamId1",
                table: "Games",
                column: "TeamId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Teams_TeamId",
                table: "Games",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Teams_TeamId1",
                table: "Games",
                column: "TeamId1",
                principalTable: "Teams",
                principalColumn: "TeamId");
        }
    }
}
