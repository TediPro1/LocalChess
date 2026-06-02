using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LocalChess.Data.Enums;

namespace LocalChess.Data.Entities
{
    public class SavedGame
    {
        public int Id { get; set; }

        public string? SaveKey { get; set; }

        public string LobbyName { get; set; } = "Local Game";

        public DateTime StartedAt { get; set; } = DateTime.Now;
        public DateTime? FinishedAt { get; set; }

        public GameResult Result { get; set; } = GameResult.Ongoing;
        public GameEndReason? EndReason { get; set; }

        public bool IsOnline { get; set; }

        public string? FinalFen { get; set; }

        public List<SavedMove> Moves { get; set; } = new();
    }
}
