using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocalChess.Server.Migrations
{
    /// <inheritdoc />
    public partial class SaveKey : Migration
    {
        /// <inheritdoc />
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

        /// <inheritdoc />
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
