using LocalChess.Controll.Interfaces;
using LocalChess.Data.DTOs;
using LocalChess.Data.Entities;
using LocalChess.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocalChess.Controll.Controllers
{

    public class OfflineLobbyManager : ILobbyManager
    {
        public event Action? LobbiesChanged;
        private readonly List<GameLobby> lobbies = new();

        public IReadOnlyList<LobbyDTO> Lobbies =>
            lobbies.Select(lobby => ToDto(lobby)).ToList();
        private static LobbyDTO ToDto(GameLobby lobby, PieceColor? assignedColor = null)
        {
            return new LobbyDTO
            {
                Id = lobby.Id,
                Name = lobby.Name,
                HasPassword = lobby.HasPassword,
                IsWaiting = lobby.IsWaiting,
                AssignedColor = assignedColor
            };
        }
        public GameLobby? GetLocalLobby(string lobbyId)
        {
            return lobbies.FirstOrDefault(l => l.Id == lobbyId);
        }
        public Task<LobbyDTO> CreateLobbyAsync(string name, string? password)
        {
            PieceColor creatorColor = Random.Shared.Next(2) == 0
                ? PieceColor.White
                : PieceColor.Black;

            var lobby = new GameLobby
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Password = string.IsNullOrWhiteSpace(password) ? null : password,
                WhiteConnected = creatorColor == PieceColor.White,
                BlackConnected = creatorColor == PieceColor.Black
            };

            lobbies.Add(lobby);
            LobbiesChanged?.Invoke();

            return Task.FromResult(ToDto(lobby, creatorColor));
        }

        public Task<LobbyDTO?> JoinLobbyAsync(string lobbyId, string? password)
        {
            GameLobby? lobby = GetLocalLobby(lobbyId);

            if (lobby == null || !lobby.IsWaiting)
                return Task.FromResult<LobbyDTO?>(null);

            if (lobby.HasPassword && lobby.Password != password)
                return Task.FromResult<LobbyDTO?>(null);

            PieceColor joinedColor;

            if (!lobby.WhiteConnected)
            {
                lobby.WhiteConnected = true;
                joinedColor = PieceColor.White;
            }
            else if (!lobby.BlackConnected)
            {
                lobby.BlackConnected = true;
                joinedColor = PieceColor.Black;
            }
            else
            {
                return Task.FromResult<LobbyDTO?>(null);
            }

            LobbiesChanged?.Invoke();
            lobby.NotifyPlayersReady();

            return Task.FromResult<LobbyDTO?>(ToDto(lobby, joinedColor));
        }

        public Task LeaveLobbyAsync(string lobbyId, PieceColor color)
        {
            GameLobby? lobby = GetLocalLobby(lobbyId);

            if (lobby == null)
                return Task.CompletedTask;

            if (color == PieceColor.White)
                lobby.WhiteConnected = false;
            else
                lobby.BlackConnected = false;

            if (!lobby.WhiteConnected && !lobby.BlackConnected)
                lobbies.Remove(lobby);

            LobbiesChanged?.Invoke();

            return Task.CompletedTask;
        }
    }
}
