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
            return string.IsNullOrEmpty(Password) ? $"{Name} {(IsWaiting ? "(Waiting for opponent)" : "(Full)")}" : $"{Name} {(IsWaiting ? "(Waiting for opponent)" : "(Full)")} 🔒";
        }
    }
}
