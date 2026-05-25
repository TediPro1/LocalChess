using LocalChess.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalChess.Controll.Events
{
    public class GameEndedEventArgs : EventArgs
    {
        public GameResult Result { get; set; }
        public GameEndReason EndReason { get; set; }
    }
}
