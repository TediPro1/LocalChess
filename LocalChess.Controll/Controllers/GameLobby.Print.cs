using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalChess.Data.Entities
{
    public partial class GameLobby
    {
        public override string ToString()
        {
            return $"{Name} {(IsWaiting ? "(Waiting)" : "(Full)")} {(HasPassword ? "🔒" : "")}";
        }
    }
}
