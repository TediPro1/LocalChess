using LocalChess.Data.Enums;

namespace LocalChess.Data.DTOs
{
    public class SavedMoveDTO
    {
        public int Id { get; set; }
        public int MoveNumber { get; set; }
        public PieceColor Color { get; set; }
        public string FromSquare { get; set; } = "";
        public string ToSquare { get; set; } = "";
        public string Notation { get; set; } = "";
        public PieceType? PromotionPiece { get; set; }
        public DateTime PlayedAt { get; set; } = DateTime.Now;
    }
}
