using LocalChess.Controll.Controllers;
using LocalChess.Controll.Interfaces;
using LocalChess.Data.Entities;
using LocalChess.Data.Enums;
using System.Drawing;

namespace LocalChess.Controll.Sessions
{
    public class OfflineGameSession : IGameSession
    {
        private readonly GameLobby lobby;

        public ChessGame Game => lobby.Game;
        public PieceColor PlayerColor { get; }
        public bool ArePlayersReady => !lobby.IsWaiting;

        public string DisplayName => $"Offline Lobby {lobby.Name} ({PlayerColor})";

        public event Action? BoardChanged;
        public event Action? PlayersReady;

        private readonly ILobbyManager lobbyManager;
        private readonly RemoteGameHistoryClient? gameHistoryClient;
        public event Action<string>? GameEnded;
        private bool hasLeft;

        public async Task EndGameAsync(string message, GameResult result, GameEndReason reason)
        {
            await SaveCompletedGameOnceAsync(result, reason);
            lobby.EndGame(message);
        }

        public OfflineGameSession(ILobbyManager lobbyManager, GameLobby lobby, PieceColor playerColor, string? serverUrl = null)
        {
            this.lobbyManager = lobbyManager;
            this.lobby = lobby;

            PlayerColor = playerColor;
            gameHistoryClient = string.IsNullOrWhiteSpace(serverUrl)
                ? null
                : new RemoteGameHistoryClient(serverUrl);

            Game.BoardChanged += () => BoardChanged?.Invoke();
            lobby.GameEnded += OnLobbyGameEnded;
            lobby.PlayersReady += OnLobbyPlayersReady;

            SubscribeGameSaving();
        }

        public Task<bool> TryMoveAsync(Point from, Point to, PieceType? promotion = null)
        {
            if (!ArePlayersReady)
                return Task.FromResult(false);

            bool success = Game.TryMove(from, to);

            if (success && promotion != null)
                Game.PromotePawn(promotion.Value);

            return Task.FromResult(success);
        }

        public async Task LeaveAsync()
        {
            if (hasLeft)
                return;

            hasLeft = true;

            bool shouldAbandonGame = ArePlayersReady && !Game.WasSaved;

            lobby.GameEnded -= OnLobbyGameEnded;
            lobby.PlayersReady -= OnLobbyPlayersReady;

            if (shouldAbandonGame)
            {
                GameResult result = PlayerColor == PieceColor.White
                    ? GameResult.BlackWon
                    : GameResult.WhiteWon;

                await SaveCompletedGameOnceAsync(result, GameEndReason.Abandoned);
                lobby.EndGame($"{PlayerColor} left the game. Game abandoned.");
            }

            await lobbyManager.LeaveLobbyAsync(lobby.Id, PlayerColor);
        }
        private void OnLobbyGameEnded(string message)
        {
            GameEnded?.Invoke(message);
        }
        private void OnLobbyPlayersReady()
        {
            PlayersReady?.Invoke();
        }
        protected void SubscribeGameSaving()
        {
            Game.GameEnded += async (_, e) =>
            {
                await SaveCompletedGameOnceAsync(e.Result, e.EndReason);
            };
        }

        private async Task SaveCompletedGameOnceAsync(GameResult result, GameEndReason reason)
        {
            if (Game.WasSaved)
                return;

            if (gameHistoryClient == null)
                return;

            Game.WasSaved = true;

            try
            {
                await gameHistoryClient.SaveCompletedGameAsync(
                    Game,
                    lobby.Name,
                    false,
                    result,
                    reason
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not save offline game history: {ex.Message}");
            }
        }
    }
}
