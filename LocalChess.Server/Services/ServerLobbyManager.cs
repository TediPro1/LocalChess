using LocalChess.Controll.Interfaces;
using LocalChess.Data.DTOs;
using LocalChess.Data.Entities;
using LocalChess.Data.Enums;

namespace LocalChess.Server.Services
{
    public class ServerLobbyManager : ILobbyManager
    {
        public event Action? LobbiesChanged;

        private readonly object lobbiesLock = new();
        private readonly List<GameLobby> lobbies = new();

        public IReadOnlyList<LobbyDTO> Lobbies
        {
            get
            {
                lock (lobbiesLock)
                {
                    return lobbies.Select(lobby => ToDto(lobby)).ToList();
                }
            }
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

            LobbyDTO createdLobby;

            lock (lobbiesLock)
            {
                lobbies.Add(lobby);
                createdLobby = ToDto(lobby, creatorColor);
            }

            LobbiesChanged?.Invoke();

            return Task.FromResult(createdLobby);
        }

        public Task<LobbyDTO?> JoinLobbyAsync(string lobbyId, string? password)
        {
            LobbyDTO? joinedLobby = null;

            lock (lobbiesLock)
            {
                GameLobby? lobby = lobbies.FirstOrDefault(lobby => lobby.Id == lobbyId);

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

                joinedLobby = ToDto(lobby, joinedColor);
            }

            LobbiesChanged?.Invoke();

            return Task.FromResult<LobbyDTO?>(joinedLobby);
        }

        public Task LeaveLobbyAsync(string lobbyId, PieceColor color)
        {
            bool lobbyChanged = false;

            lock (lobbiesLock)
            {
                GameLobby? lobby = lobbies.FirstOrDefault(lobby => lobby.Id == lobbyId);

                if (lobby == null)
                    return Task.CompletedTask;

                if (color == PieceColor.White)
                    lobby.WhiteConnected = false;
                else
                    lobby.BlackConnected = false;

                if (!lobby.WhiteConnected && !lobby.BlackConnected)
                    lobbies.Remove(lobby);

                lobbyChanged = true;
            }

            if (lobbyChanged)
                LobbiesChanged?.Invoke();

            return Task.CompletedTask;
        }

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
    }
}
