using LocalChess.Controll.Controllers;
using LocalChess.Controll.Interfaces;
using LocalChess.Data.DTOs;
using LocalChess.Data.Entities;
using LocalChess.Data.Enums;
using Microsoft.AspNetCore.SignalR.Client;
using System.Drawing;

namespace LocalChess.Controll.Sessions
{
    public class OnlineGameSession : IGameSession
    {
        public ChessGame Game { get; } = new ChessGame();
        public PieceColor PlayerColor { get; }
        public bool ArePlayersReady => arePlayersReady;

        private readonly HubConnection connection;
        private readonly RemoteGameHistoryClient gameHistoryClient;
        private readonly string lobbyId;
        private readonly string lobbyName;
        public string DisplayName => $"Online Lobby - {lobbyName} ({PlayerColor})";

        public event Action? BoardChanged;
        public event Action? PlayersReady;
        public event Action<string>? GameEnded;
        private bool hasLeft;
        private bool arePlayersReady;

        public OnlineGameSession(string serverUrl, string lobbyId, PieceColor playerColor, bool arePlayersReady = false, string? lobbyName = null)
        {
            this.lobbyId = lobbyId;
            this.lobbyName = string.IsNullOrWhiteSpace(lobbyName) ? lobbyId : lobbyName;
            PlayerColor = playerColor;
            this.arePlayersReady = arePlayersReady;
            gameHistoryClient = new RemoteGameHistoryClient(serverUrl);

            connection = new HubConnectionBuilder()
                .WithUrl($"{serverUrl}/chesshub")
                .WithAutomaticReconnect()
                .Build();

            connection.On<MoveDTO>("ReceiveMove", move =>
            {
                Console.WriteLine("Received move from server");

                Game.TryMove(
                    new Point(move.FromRow, move.FromCol),
                    new Point(move.ToRow, move.ToCol)
                );

                if (move.PromotionChoice != null)
                    Game.PromotePawn(move.PromotionChoice.Value);

                BoardChanged?.Invoke();
            });

            connection.On("PlayersReady", NotifyPlayersReady);

            connection.On<GameEndedDTO>("GameEnded", async dto =>
            {
                await SaveCompletedGameOnceAsync(dto.Result, dto.EndReason);
                GameEnded?.Invoke(dto.Message);
            });

            SubscribeGameSaving();
        }

        private void NotifyPlayersReady()
        {
            if (arePlayersReady)
                return;

            arePlayersReady = true;
            PlayersReady?.Invoke();
        }

        public async Task StartAsync()
        {
            await connection.StartAsync();
            await connection.InvokeAsync("JoinGameGroup", lobbyId, PlayerColor);
        }

        public async Task<bool> TryMoveAsync(Point from, Point to, PieceType? promotion = null)
        {
            if (!arePlayersReady)
                return false;

            bool success = Game.TryMove(from, to);

            if (!success)
                return false;

            if (promotion != null)
                Game.PromotePawn(promotion.Value);

            BoardChanged?.Invoke();

            await connection.InvokeAsync("SendMove", new MoveDTO
            {
                LobbyId = lobbyId,
                FromRow = from.X,
                FromCol = from.Y,
                ToRow = to.X,
                ToCol = to.Y,
                PromotionChoice = promotion
            }).WaitAsync(TimeSpan.FromSeconds(3));

            return true;
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

            Game.WasSaved = true;

            try
            {
                await gameHistoryClient.SaveCompletedGameAsync(
                    Game,
                    lobbyName,
                    true,
                    result,
                    reason,
                    $"online-lobby:{lobbyId}"
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not save online game history: {ex.Message}");
            }
        }

        public async Task EndGameAsync(string message, GameResult result, GameEndReason reason)
        {
            await SaveCompletedGameOnceAsync(result, reason);

            await connection.InvokeAsync("EndGame", new GameEndedDTO
            {
                LobbyId = lobbyId,
                Message = message,
                Result = result,
                EndReason = reason
            });
        }
        public async Task LeaveAsync()
        {
            if (hasLeft)
                return;

            hasLeft = true;

            if (connection.State == HubConnectionState.Connected)
            {
                await connection.InvokeAsync("LeaveLobby", lobbyId, PlayerColor);
                await connection.InvokeAsync("LeaveGameGroup", lobbyId);
            }

            await connection.DisposeAsync();
        }
    }
}
