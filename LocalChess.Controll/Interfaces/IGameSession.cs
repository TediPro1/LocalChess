using LocalChess.Data.Entities;
using LocalChess.Data.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalChess.Controll.Interfaces
{
    public interface IGameSession
    {
        ChessGame Game { get; }
        PieceColor PlayerColor { get; }
        string DisplayName { get; }
        bool ArePlayersReady { get; }
        event Action? BoardChanged;
        event Action? PlayersReady;
        event Action<string>? GameEnded;
        Task EndGameAsync(string message, GameResult result, GameEndReason reason);
        Task<bool> TryMoveAsync(Point from, Point to, PieceType? promotion = null);
        Task LeaveAsync();
    }
}
