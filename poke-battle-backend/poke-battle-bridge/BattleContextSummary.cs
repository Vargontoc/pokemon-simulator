using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace poke.battle.bridge
{
    public class BattleContextSummary
    {
        public string Player { get; set; } = string.Empty;
        public int PlayerHp { get; set; }
        public List<string> PlayerMoves { get; set; } = [];
        public string Opponent { get; set; } = string.Empty;
        public int OpponentHp { get; set; }
        public List<string> OpponentMoves { get; set; } = [];
    }
}
