using LocalChess.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalChess.Data.Entities
{
    public class ChessPiece
    {
        public PieceType Type
        {
            get; set;
        }
        public PieceColor Color
        {
            get; set;
        }
        public bool HasMoved
        {
            get; set;
        }

        public ChessPiece(PieceType type, PieceColor color)
        {
            Type = type;
            Color = color;
        }
    }
}
