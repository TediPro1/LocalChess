using LocalChess.Data.Enums;

namespace LocalChess.Data.DTOs
{
    public class SavedGameDTO
    {
        public int Id { get; set; }
        public string LobbyName { get; set; } = "Local Game";
        public DateTime StartedAt { get; set; } = DateTime.Now;
        public DateTime? FinishedAt { get; set; }
        public GameResult Result { get; set; } = GameResult.Ongoing;
        public GameEndReason? EndReason { get; set; }
        public bool IsOnline { get; set; }
        public string? FinalFen { get; set; }
        public List<SavedMoveDTO> Moves { get; set; } = new();
    }
}
