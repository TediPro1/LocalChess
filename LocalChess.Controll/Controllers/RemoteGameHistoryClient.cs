using LocalChess.Data.DTOs;
using LocalChess.Data.Entities;
using LocalChess.Data.Enums;
using System.Net.Http.Json;

namespace LocalChess.Controll.Controllers
{
    public class RemoteGameHistoryClient
    {
        private readonly HttpClient httpClient;

        public RemoteGameHistoryClient(string serverUrl)
        {
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(serverUrl.TrimEnd('/') + "/")
            };
        }

        public async Task<List<SavedGameDTO>> GetSavedGamesAsync()
        {
            return await httpClient.GetFromJsonAsync<List<SavedGameDTO>>("saved-games")
                ?? new List<SavedGameDTO>();
        }

        public async Task SaveCompletedGameAsync(
            ChessGame game,
            string lobbyName,
            bool isOnline,
            GameResult result,
            GameEndReason reason,
            string? saveKey = null)
        {
            var dto = new CompletedGameDTO
            {
                SaveKey = saveKey,
                LobbyName = lobbyName,
                StartedAt = game.StartedAt,
                Result = result,
                EndReason = reason,
                IsOnline = isOnline,
                FinalFen = game.ToFen(),
                Moves = game.MoveHistory
                    .Select(move => new SavedMoveDTO
                    {
                        MoveNumber = move.MoveNumber,
                        Color = move.Color,
                        FromSquare = move.FromSquare,
                        ToSquare = move.ToSquare,
                        Notation = move.Notation,
                        PromotionPiece = move.PromotionPiece,
                        PlayedAt = move.PlayedAt
                    })
                    .ToList()
            };

            using HttpResponseMessage response = await httpClient.PostAsJsonAsync("saved-games", dto);
            response.EnsureSuccessStatusCode();
        }
    }
}
