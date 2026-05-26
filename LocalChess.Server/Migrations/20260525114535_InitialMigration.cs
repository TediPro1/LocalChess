using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocalChess.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SavedGames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LobbyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Result = table.Column<int>(type: "int", nullable: false),
                    EndReason = table.Column<int>(type: "int", nullable: true),
                    IsOnline = table.Column<bool>(type: "bit", nullable: false),
                    FinalFen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedGames", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SavedMoves",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SavedGameId = table.Column<int>(type: "int", nullable: false),
                    MoveNumber = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<int>(type: "int", nullable: false),
                    FromSquare = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    ToSquare = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Notation = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PromotionPiece = table.Column<int>(type: "int", nullable: true),
                    PlayedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedMoves", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavedMoves_SavedGames_SavedGameId",
                        column: x => x.SavedGameId,
                        principalTable: "SavedGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SavedMoves_SavedGameId",
                table: "SavedMoves",
                column: "SavedGameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SavedMoves");

            migrationBuilder.DropTable(
                name: "SavedGames");
        }
    }
}
