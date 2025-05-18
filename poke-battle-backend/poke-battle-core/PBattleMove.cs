using poke.battle.Models;

namespace poke.battle.core
{
    public class PBattleMove 
    {
        public MoveModel Move { get; }
        public int CurrentPP {get; set; }
        public int MaxPP => Move.PP;
        public bool UsedThisTurn {get; set;} = false;
        public bool IsDisabled {get { return DisablesTurns > 0;} }
        public int DisablesTurns { get; private set; } = 0; 

        public PBattleMove(MoveModel move) {
            Move = move;
            CurrentPP = move.PP;
        }

        public void Use() {
            if(CurrentPP > 0)
                CurrentPP--;
            UsedThisTurn = true;
        }

        public void Reset() => UsedThisTurn = false;

        public void DisableMove(int turns) => DisablesTurns = turns;
        public void TickDisable()
        {
            if(DisablesTurns > 0)
            {
                DisablesTurns--;
            }
        }
    }




}