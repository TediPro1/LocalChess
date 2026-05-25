using LocalChess.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalChess.Data.DTOs
{
    public class MoveDTO
    {
        public string LobbyId { get; set; } = "";

        public int FromRow { get; set; }
        public int FromCol { get; set; }

        public int ToRow { get; set; }
        public int ToCol { get; set; }

        public PieceType? PromotionChoice { get; set; }
    }
}
