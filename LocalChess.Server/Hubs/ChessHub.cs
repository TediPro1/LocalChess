using LocalChess.Controll.Controllers;
using LocalChess.Controll.Interfaces;
using LocalChess.Data.DTOs;
using LocalChess.Data.Entities;
using LocalChess.Data.Enums;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace LocalChess.Server.Hubs
{
    public class ChessHub : Hub
    {
        private static readonly ConcurrentDictionary<string, (string LobbyId, PieceColor Color)> connectionPlayers = new();
        private readonly ILobbyManager lobbyManager;

        public ChessHub(ILobbyManager lobbyManager)
        {
            this.lobbyManager = lobbyManager;
        }

        public Task<List<LobbyDTO>> GetLobbies()
        {
            return Task.FromResult(lobbyManager.Lobbies.ToList());
        }

        public async Task<LobbyDTO> CreateLobby(string name, string? password)
        {
            LobbyDTO lobby = await lobbyManager.CreateLobbyAsync(name, password);

            await Clients.All.SendAsync("LobbiesUpdated", lobbyManager.Lobbies.ToList());

            return lobby;
        }

        public async Task<LobbyDTO?> JoinLobby(string lobbyId, string? password)
        {
            LobbyDTO? lobby = await lobbyManager.JoinLobbyAsync(lobbyId, password);

            await Clients.All.SendAsync("LobbiesUpdated", lobbyManager.Lobbies.ToList());

            if (lobby != null && !lobby.IsWaiting)
                await Clients.Group(lobbyId).SendAsync("PlayersReady");

            return lobby;
        }

        public async Task LeaveLobby(string lobbyId, PieceColor color)
        {
            connectionPlayers.TryRemove(Context.ConnectionId, out _);
            await LeaveLobbyCoreAsync(lobbyId, color);
        }

        public async Task JoinGameGroup(string lobbyId, PieceColor color)
        {
            connectionPlayers[Context.ConnectionId] = (lobbyId, color);
            await Groups.AddToGroupAsync(Context.ConnectionId, lobbyId);
        }
        public async Task LeaveGameGroup(string lobbyId)
        {
            connectionPlayers.TryRemove(Context.ConnectionId, out _);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, lobbyId);
        }

        public async Task SendMove(MoveDTO move)
        {
            await Clients.OthersInGroup(move.LobbyId)
                .SendAsync("ReceiveMove", move);
        }
        public async Task EndGame(GameEndedDTO dto)
        {
            await Clients.Group(dto.LobbyId)
                .SendAsync("GameEnded", dto);

            await lobbyManager.LeaveLobbyAsync(dto.LobbyId, PieceColor.White);
            await lobbyManager.LeaveLobbyAsync(dto.LobbyId, PieceColor.Black);

            await Clients.All.SendAsync("LobbiesUpdated", lobbyManager.Lobbies.ToList());
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (connectionPlayers.TryRemove(Context.ConnectionId, out var player))
                await LeaveLobbyCoreAsync(player.LobbyId, player.Color);

            await base.OnDisconnectedAsync(exception);
        }

        private async Task LeaveLobbyCoreAsync(string lobbyId, PieceColor color)
        {
            bool wasInProgress = lobbyManager.Lobbies
                .Any(lobby => lobby.Id == lobbyId && !lobby.IsWaiting);

            await lobbyManager.LeaveLobbyAsync(lobbyId, color);

            if (wasInProgress)
            {
                GameResult result = color == PieceColor.White
                    ? GameResult.BlackWon
                    : GameResult.WhiteWon;

                await Clients.Group(lobbyId).SendAsync("GameEnded", new GameEndedDTO
                {
                    LobbyId = lobbyId,
                    Result = result,
                    EndReason = GameEndReason.Abandoned,
                    Message = $"{color} left the game. Game abandoned."
                });
            }

            await Clients.All.SendAsync("LobbiesUpdated", lobbyManager.Lobbies.ToList());
        }
    }
}
