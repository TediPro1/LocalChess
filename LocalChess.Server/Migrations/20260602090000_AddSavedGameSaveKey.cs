using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocalChess.Server.Migrations
{
    public partial class AddSavedGameSaveKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SaveKey",
                table: "SavedGames",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SavedGames_SaveKey",
                table: "SavedGames",
                column: "SaveKey",
                unique: true,
                filter: "[SaveKey] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SavedGames_SaveKey",
                table: "SavedGames");

            migrationBuilder.DropColumn(
                name: "SaveKey",
                table: "SavedGames");
        }
    }
}
