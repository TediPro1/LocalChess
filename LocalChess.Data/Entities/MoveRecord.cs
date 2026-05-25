using LocalChess.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalChess.Data.Entities
{
    public class MoveRecord
    {
        public int MoveNumber { get; set; }
        public PieceColor Color { get; set; }
        public string FromSquare { get; set; } = "";
        public string ToSquare { get; set; } = "";
        public string Notation { get; set; } = "";
        public PieceType? PromotionPiece { get; set; }
        public DateTime PlayedAt { get; set; } = DateTime.Now;
    }
}
