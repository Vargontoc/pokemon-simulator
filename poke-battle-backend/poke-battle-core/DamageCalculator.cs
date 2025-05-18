using System.ComponentModel;
using System.Runtime.CompilerServices;
using poke.battle.Models;
using poke.battle.Models.helpers;

namespace poke.battle.core
{
    public static class DamageCalculator {
        private static readonly Random rng = new();

        public static int Calculate(PBattler attacker, PBattler defender, MoveModel move, BattleContext context, out double effectiveness, out bool isCritical) {
            if(move.Power == 0 && !IsFixedDamage(move.MoveEffect)) 
            {
                isCritical = false;
                effectiveness = 1.0;
                return 0;
            }
            
            isCritical = IsCriticalHit(attacker);
            int level = attacker.Level;

            bool isPhysical = move.MoveType == MoveType.Physical;
            var atkStat = isPhysical ? Stat.Attk : Stat.AtkSp;
            var defStat = isPhysical ? Stat.Def : Stat.DefSp;

            double atk = attacker.GetStat(atkStat).ModValue;
            double def = defender.GetStat(defStat).ModValue;

            if (isPhysical && attacker.HasStatus(PStatus.Burn))
                atk /= 2;

            if (!isCritical)
            {
                if (isPhysical && context.GetSide(defender).HasEffect("reflect"))
                    def *= 2;
                else if (!isPhysical && context.GetSide(defender).HasEffect("light-screen"))
                    def *= 2;
            }

            switch(move.MoveEffect)
            {
                case var code when code == MoveEffect.Fixed_20.Code:
                    effectiveness = 1.0;
                    isCritical = false;
                    return 20;

                case var code when code == MoveEffect.Fixed_40.Code:
                    effectiveness = 1.0;
                    isCritical = false;
                    return 40;


                case var code when code == MoveEffect.LevelDamage.Code:
                    effectiveness = 1.0;
                    isCritical = false;
                    return attacker.Level;

                case var code when code == MoveEffect.RandomLevel.Code:
                    effectiveness = 1.0;
                    isCritical = false;
                    return rng.Next((int)(attacker.Level * .5), (int)(attacker.Level * 1.5));

            }

            double damage = (((2 * level / 5.0 + 2) * move.Power * (atk / def)) /50.0) + 2;

            effectiveness = GetTypeEffectiveness(move, defender);
            
            if(isCritical)
                damage = (int)(damage * 1.5);

            double modifier = GetSTAB(attacker, move) 
                                * effectiveness
                                * GetRandomFactor();

            return (int)(damage * modifier);
        }

        private static bool IsFixedDamage(int effect)
        {
            return effect == MoveEffect.Fixed_40.Code || effect == MoveEffect.LevelDamage.Code || effect == MoveEffect.Fixed_20.Code || effect == MoveEffect.RandomLevel.Code;
        }

        public static int ConfusionDamage(int level, int attk, int def) {
            int power = 40;
            int numerator = ((2 * level / 5 +2) * power * attk);
            int damage = (numerator / def) / 50 + 2;
            return Math.Max(1, damage);
        }

        private static readonly double[] CritChances = [ 1.0 / 24.0, 1.0 / 8.0, 0.5, 1.0, 1.0];
        private static bool IsCriticalHit(PBattler attacker) {
            int crit  = Math.Clamp(attacker.Critical, 0, 4);
            return new Random().NextDouble() < CritChances[crit];
        }
        
        private static readonly double[] Multipliers =
        [ 1.0 / 3.0, 2.0 / 5.0, 1.0 / 2.0, 2.0 / 3.0, 1.0, 3.0 / 2.0, 2.0, 5.0 / 2.0, 3.0];

        private static double GetMultiplier(int stage) => Multipliers[Math.Clamp(stage + 6, 0, 12)];
        private static double GetTypeEffectiveness(MoveModel move, PBattler defender) => TypeEffectivenessResolver.GetEffectivenessAgainst(move.Type, defender.Specie.Types);
        private static double GetSTAB(PBattler attacker, MoveModel move)  => attacker.Specie.Types.Contains(move.Type) ? 1.5 : 1.0;
        private static double GetCriticalMultiplier() => rng.NextDouble()  < 0.0625 ? 1.5: 1.0;
        private static double GetRandomFactor() => rng.Next(85, 100) / 100.0;
    }
}