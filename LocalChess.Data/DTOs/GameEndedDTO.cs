using LocalChess.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalChess.Data.DTOs
{
    public class GameEndedDTO
    {
        public string LobbyId { get; set; } = "";
        public GameResult Result { get; set; }
        public GameEndReason EndReason { get; set; }
        public string Message { get; set; } = "";
    }
}
