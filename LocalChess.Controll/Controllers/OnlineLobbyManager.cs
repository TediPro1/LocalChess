using LocalChess.Controll.Interfaces;
using LocalChess.Data.DTOs;
using LocalChess.Data.Entities;
using LocalChess.Data.Enums;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalChess.Controll.Controllers
{
    public class OnlineLobbyManager : ILobbyManager
    {
        private readonly HubConnection connection;
        public event Action? LobbiesChanged;
        private readonly List<LobbyDTO> lobbies = new();
        private readonly object lobbiesLock = new();

        public IReadOnlyList<LobbyDTO> Lobbies
        {
            get
            {
                lock (lobbiesLock)
                {
                    return lobbies.ToList();
                }
            }
        }

        public OnlineLobbyManager(string serverUrl)
        {
            connection = new HubConnectionBuilder()
                .WithUrl($"{serverUrl}/chesshub")
                .WithAutomaticReconnect()
                .Build();

            connection.On<List<LobbyDTO>>("LobbiesUpdated", updatedLobbies =>
            {
                UpdateLobbies(updatedLobbies);
                LobbiesChanged?.Invoke();
            });
        }

        public async Task StartAsync()
        {
            if (connection.State == HubConnectionState.Disconnected)
                await connection.StartAsync();

            await RefreshLobbiesAsync();
        }

        public async Task RefreshLobbiesAsync()
        {
            var updatedLobbies = await connection.InvokeAsync<List<LobbyDTO>>("GetLobbies");

            UpdateLobbies(updatedLobbies);

            LobbiesChanged?.Invoke();
        }

        public async Task<LobbyDTO> CreateLobbyAsync(string name, string? password)
        {
            await EnsureConnectedAsync();

            LobbyDTO lobby = await connection.InvokeAsync<LobbyDTO>(
                "CreateLobby",
                name,
                password
            );

            await RefreshLobbiesAsync();

            return lobby;
        }

        public async Task<LobbyDTO?> JoinLobbyAsync(string lobbyId, string? password)
        {
            await EnsureConnectedAsync();

            LobbyDTO? lobby = await connection.InvokeAsync<LobbyDTO?>(
                "JoinLobby",
                lobbyId,
                password
            );

            await RefreshLobbiesAsync();

            return lobby;
        }

        public async Task LeaveLobbyAsync(string lobbyId, PieceColor color)
        {
            await EnsureConnectedAsync();

            await connection.InvokeAsync(
                "LeaveLobby",
                lobbyId,
                color
            );

            await RefreshLobbiesAsync();
        }

        private async Task EnsureConnectedAsync()
        {
            if (connection.State == HubConnectionState.Disconnected)
                await connection.StartAsync();
        }

        private void UpdateLobbies(IEnumerable<LobbyDTO> updatedLobbies)
        {
            lock (lobbiesLock)
            {
                lobbies.Clear();
                lobbies.AddRange(updatedLobbies);
            }
        }
    }
}
