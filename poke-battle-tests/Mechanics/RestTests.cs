using poke.battle.core;
using poke.battle.Models.helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace poke_battle_tests.Mechanics
{
    public class RestTests
    {
        [Fact]
        public void Rest_ShouldHealAndSleep()
        {
            var attacker = Utils.GetBattler("bulbasaur", 50);
            attacker.IsActive = true;
            attacker.CurrentHp = 20;

            var defender = Utils.GetBattler("charmander", 50);
            defender.IsActive = true;

            var protect = Utils.GetMove("rest");
            protect.Move.MoveEffect = MoveEffect.Rest;

            var context = new BattleContext([attacker], [defender]);
            var action = new MoveAction() { Actor = attacker, Move = protect, Context = context };
            var result = action.Execute(defender);

            int expected = attacker.GetStat(Stat.Hp).RawValue;
            Assert.Equal(expected, attacker.CurrentHp);

            Assert.Equal(PStatus.Sleep, attacker.Status);
        }
    }
}
