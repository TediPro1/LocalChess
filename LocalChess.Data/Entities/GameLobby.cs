using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalChess.Data.Entities
{
    public partial class GameLobby
    {
        public string Name { get; set; }
        public string? Password { get; set; }

        public ChessGame Game { get; } = new ChessGame();

        public bool WhiteConnected { get; set; }
        public bool BlackConnected { get; set; }

        public bool IsWaiting => !BlackConnected;
    }
}
