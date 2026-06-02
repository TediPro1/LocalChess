using LocalChess.Data.DTOs;
using LocalChess.Data.Entities;
using LocalChess.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace LocalChess.Server.Services
{
    public class GameSaveService
    {
        private readonly ChessContext context;

        public GameSaveService(ChessContext context)
        {
            this.context = context;
        }

        public async Task<SavedGameDTO> SaveGameAsync(CompletedGameDTO completedGame)
        {
            SavedGame? existingGame = await FindExistingGameAsync(completedGame.SaveKey);

            if (existingGame != null)
                return ToDto(existingGame);

            var savedGame = new SavedGame
            {
                SaveKey = completedGame.SaveKey,
                LobbyName = completedGame.LobbyName,
                StartedAt = completedGame.StartedAt,
                FinishedAt = DateTime.Now,
                Result = completedGame.Result,
                EndReason = completedGame.EndReason,
                IsOnline = completedGame.IsOnline,
                FinalFen = completedGame.FinalFen
            };

            foreach (SavedMoveDTO move in completedGame.Moves)
            {
                savedGame.Moves.Add(new SavedMove
                {
                    MoveNumber = move.MoveNumber,
                    Color = move.Color,
                    FromSquare = move.FromSquare,
                    ToSquare = move.ToSquare,
                    Notation = move.Notation,
                    PromotionPiece = move.PromotionPiece,
                    PlayedAt = move.PlayedAt
                });
            }

            context.SavedGames.Add(savedGame);

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException) when (!string.IsNullOrWhiteSpace(completedGame.SaveKey))
            {
                context.ChangeTracker.Clear();

                existingGame = await FindExistingGameAsync(completedGame.SaveKey);

                if (existingGame != null)
                    return ToDto(existingGame);

                throw;
            }

            return ToDto(savedGame);
        }

        private async Task<SavedGame?> FindExistingGameAsync(string? saveKey)
        {
            if (string.IsNullOrWhiteSpace(saveKey))
                return null;

            return await context.SavedGames
                .Include(game => game.Moves)
                .FirstOrDefaultAsync(game => game.SaveKey == saveKey);
        }

        public static SavedGameDTO ToDto(SavedGame game)
        {
            return new SavedGameDTO
            {
                Id = game.Id,
                LobbyName = game.LobbyName,
                StartedAt = game.StartedAt,
                FinishedAt = game.FinishedAt,
                Result = game.Result,
                EndReason = game.EndReason,
                IsOnline = game.IsOnline,
                FinalFen = game.FinalFen,
                Moves = game.Moves
                    .OrderBy(move => move.MoveNumber)
                    .ThenBy(move => move.Id)
                    .Select(ToDto)
                    .ToList()
            };
        }

        private static SavedMoveDTO ToDto(SavedMove move)
        {
            return new SavedMoveDTO
            {
                Id = move.Id,
                MoveNumber = move.MoveNumber,
                Color = move.Color,
                FromSquare = move.FromSquare,
                ToSquare = move.ToSquare,
                Notation = move.Notation,
                PromotionPiece = move.PromotionPiece,
                PlayedAt = move.PlayedAt
            };
        }
    }
}
