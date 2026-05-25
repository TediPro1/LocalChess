using LocalChess.Controll.Controllers;
using LocalChess.Controll.Interfaces;
using LocalChess.Data.DTOs;
using LocalChess.Data.Entities;
using LocalChess.Data.Enums;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace LocalChess.Server.Hubs
{
    public class ChessHub : Hub
    {
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

            return lobby;
        }

        public async Task LeaveLobby(string lobbyId, PieceColor color)
        {
            await lobbyManager.LeaveLobbyAsync(lobbyId, color);

            await Clients.All.SendAsync("LobbiesUpdated", lobbyManager.Lobbies.ToList());
        }

        public async Task JoinGameGroup(string lobbyId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, lobbyId);
        }
        public async Task LeaveGameGroup(string lobbyId)
        {
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
    }
}
