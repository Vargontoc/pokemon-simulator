using System.Net;
using poke.battle.Models.Impl;

namespace poke.battle.core
{
    public static class BattleMath 
    {
        private static readonly Random rng = new();
        
        public static bool ApplyAccuracy(PBattler attacker, PBattler defender, int moveAccuracy) 
        {
            if(moveAccuracy >= 100 || moveAccuracy <= 0) return true;

            double accMod = attacker.GetAccuracy();
            double evaMod = defender.GetEvassion();

            double acc = moveAccuracy * (accMod / evaMod);

            int roll =  rng.Next(1, 101);
            return roll <= acc;
        }


    }
}