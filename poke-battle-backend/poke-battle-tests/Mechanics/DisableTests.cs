using poke.battle.core;
using poke.battle.Models.helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace poke_battle_tests.Mechanics
{
    public class DisableTests
    {
        [Fact]
        public void Disable_BlockMove()
        {
            var attacker = Utils.GetBattler("alakazam", 50);
            attacker.IsActive = true;

            var defender = Utils.GetBattler("pikachu", 50);
            defender.IsActive = true;

            var thunderbolt = Utils.GetMove("thunderbolt");
            defender.SetMoves([thunderbolt]);

            var disable = Utils.GetMove("disable");
            disable.Move.MoveEffect = MoveEffect.Disable;

            var context = new BattleContext([attacker], [defender]);
            var action = new MoveAction() {  Actor = attacker, Move = disable, Context = context };

            var result = action.Execute(defender);
            Assert.Contains(defender.Moves, m => m.IsDisabled);
            Assert.True(defender.Moves.First().DisablesTurns is >= 4 and <= 7);

            for (int i = 0; i < 10; i++)
                defender.OnTurnEnd(result, context);

            Assert.False(defender.Moves.First().IsDisabled);

        }

    }
}
