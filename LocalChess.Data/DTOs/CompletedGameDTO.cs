using LocalChess.Data.Enums;

namespace LocalChess.Data.DTOs
{
    public class CompletedGameDTO
    {
        public string? SaveKey { get; set; }
        public string LobbyName { get; set; } = "Local Game";
        public DateTime StartedAt { get; set; } = DateTime.Now;
        public GameResult Result { get; set; }
        public GameEndReason EndReason { get; set; }
        public bool IsOnline { get; set; }
        public string? FinalFen { get; set; }
        public List<SavedMoveDTO> Moves { get; set; } = new();
    }
}
