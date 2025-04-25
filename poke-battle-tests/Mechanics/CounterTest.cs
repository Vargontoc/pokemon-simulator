using poke.battle.core;
using poke.battle.Models.helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace poke_battle_tests.Mechanics
{
    public class CounterTest
    {
        [Fact]
        public void Counter_ShouldReflectDamage()
        {
            var attacker = Utils.GetBattler("machop", 50);
            var defender = Utils.GetBattler("hitmonlee", 50);
            attacker.IsActive = true;
            defender.IsActive = true;

            defender.LastPhysicalDamage = 20;
            defender.LastAttacker = attacker;

            var counter = Utils.GetMove("counter");
            counter.Move.MoveEffect = MoveEffect.Counter;

            var context = new BattleContext([attacker], [defender]);
            var action = new MoveAction()
            {
                Actor = defender,
                Move = counter,
                Context = context
            };

            var result = action.Execute(attacker);

            int expected = 40;
            Assert.Contains(result.Events, e => e.Type == "damage");
            Assert.Equal(attacker.MaxHP - expected, attacker.CurrentHp);

        }
    }
}
