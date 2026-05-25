using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LocalChess.Controll.Controllers;

namespace LocalChess.Data.Entities
{
    public partial class GameLobby
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Password { get; set; }

        public ChessGame Game { get; } = new ChessGame();

        public bool WhiteConnected { get; set; }
        public bool BlackConnected { get; set; }

        public bool IsWaiting => !BlackConnected;
        public bool HasPassword => !string.IsNullOrWhiteSpace(Password);

        public event Action<string>? GameEnded;

        public void EndGame(string message)
        {
            GameEnded?.Invoke(message);
        }
    }
}
