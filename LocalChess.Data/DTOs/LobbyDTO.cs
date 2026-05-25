using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalChess.Data.DTOs
{
    public class LobbyDTO
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public bool HasPassword { get; set; }
        public bool IsWaiting { get; set; }

        public override string ToString()
        {
            return $"{Name} {(IsWaiting ? "(Waiting)" : "(Full)")} {(HasPassword ? "🔒" : "")}";
        }
    }
}
