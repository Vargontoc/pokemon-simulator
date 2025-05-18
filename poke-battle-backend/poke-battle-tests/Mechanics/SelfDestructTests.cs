using poke.battle.core;
using poke.battle.Models.helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace poke_battle_tests.Mechanics
{
    public class SelfDestructTests
    {
        [Fact]
        public void SelfDestruct_DDealsDamageAndFaintUser()
        {
            var attacker = Utils.GetBattler("bulbasaur", 50);
            attacker.IsActive = true;
            attacker.CurrentHp = 20;

            var defender = Utils.GetBattler("charmander", 50);
            defender.IsActive = true;

            var protect = Utils.GetMove("self-destruct");
            protect.Move.MoveEffect = MoveEffect.SelfDestruct;

            var context = new BattleContext([attacker], [defender]);
            var action = new MoveAction() { Actor = attacker, Move = protect, Context = context };
            var result = action.Execute(defender);

            int expected = attacker.GetStat(Stat.Hp).RawValue / 2;
            Assert.True(attacker.IsFainted);
            Assert.True(defender.CurrentHp < defender.GetStat(Stat.Hp).RawValue);
            Assert.Contains(result.Events, e => e.Type == "self-ko");
        }
    }
}
