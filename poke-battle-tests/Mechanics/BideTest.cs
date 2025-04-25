using poke.battle.core;
using poke.battle.Models.helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace poke_battle_tests.Mechanics
{
    public class BideTest
    {
        [Fact]
        public void Bide_ShouldAccumulate_Release()
        {
            var attacker = Utils.GetBattler("bulbasaur", 50);
            var defender = Utils.GetBattler("charmander", 50);
            attacker.IsActive = true;
            defender.IsActive = true;

            var bide = Utils.GetMove("bide");
            bide.Move.MoveEffect = MoveEffect.Bide;

            var tackle = Utils.GetMove("tackle");

            var context = new BattleContext([attacker], [defender]);

            // Turn 1
            var turn1 = new BattleTurn(context, [], 1);
            turn1.RegisterAction(new MoveAction() { Actor = attacker, Move = bide, Context = context });
            turn1.RegisterAction(new MoveAction() { Actor = defender, Move = tackle, Context = context });
            turn1.ExecuteTurn();

            Assert.True(attacker.IsBiding);
            Assert.True(attacker.BideDamage > 0);
            Assert.NotNull(attacker.BideLastAttacker);

            // Turn 2
            var turn2 = new BattleTurn(context, [], 2);
            turn2.RegisterAction(new MoveAction { Actor = attacker, Move = bide, Context = context });
            turn2.RegisterAction(new MoveAction { Actor = defender, Move = tackle, Context = context });
            turn2.ExecuteTurn();

            Assert.True(attacker.IsBiding);

            //  Turn 3
            var hpBefore = defender.CurrentHp;
            var turn3 = new BattleTurn(context, [], 3);
            turn3.RegisterAction(new MoveAction { Actor = attacker, Move = bide, Context = context });
            turn3.ExecuteTurn(); Assert.True(attacker.IsBiding);

            var hpAfter = defender.CurrentHp;
            int damageDealt = hpBefore - hpAfter;

            Assert.False(attacker.IsBiding);
            Assert.True(damageDealt >= attacker.BideDamage * 2 || damageDealt > 0);


        }
    }
}
