using LocalChess.Data.DTOs;
using LocalChess.Data.Entities;
using LocalChess.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalChess.Controll.Interfaces
{
    public interface ILobbyManager
    {
        event Action? LobbiesChanged;

        IReadOnlyList<LobbyDTO> Lobbies { get; }

        Task<LobbyDTO> CreateLobbyAsync(string name, string? password);
        Task<LobbyDTO?> JoinLobbyAsync(string lobbyId, string? password);
        Task LeaveLobbyAsync(string lobbyID, PieceColor color);
    }
}
