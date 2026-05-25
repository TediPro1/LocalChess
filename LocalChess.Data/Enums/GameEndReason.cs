using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalChess.Data.Enums
{
    public enum GameEndReason
    {
        Checkmate,
        Stalemate,
        InsufficientMaterial,
        Repetition,
        Resignation,
        Abandoned
    }
}
