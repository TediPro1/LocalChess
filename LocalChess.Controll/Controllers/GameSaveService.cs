using LocalChess.Data.Data;
using LocalChess.Data.Entities;
using LocalChess.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalChess.Controll.Controllers
{
    public class GameSaveService
    {
        private readonly ChessContext context;

        public GameSaveService(ChessContext context)
        {
            this.context = context;
        }

        public async Task SaveGameAsync(
            ChessGame game,
            string lobbyName,
            bool isOnline,
            GameResult result,
            GameEndReason reason)
        {
            var savedGame = new SavedGame
            {
                LobbyName = lobbyName,
                StartedAt = game.StartedAt,
                FinishedAt = DateTime.Now,
                Result = result,
                EndReason = reason,
                IsOnline = isOnline,
                FinalFen = game.ToFen()
            };

            foreach (var move in game.MoveHistory)
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
            await context.SaveChangesAsync();
        }
    }
}
