using poke.battle.core;
using poke.battle.Models.helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace poke_battle_tests.Mechanics
{
    public class MirrorWaveTests
    {
        [Fact]
        public void MirrorWave_ShouldCopyLastMove()
        {
            var p1 = Utils.GetBattler("pidgey", 50);
            p1.IsActive = true;

            var p2 = Utils.GetBattler("bulbasaur", 50);
            p2.IsActive = true;

            var tackle = Utils.GetMove("tackle");
            tackle.Move.MoveEffect = MoveEffect.None;

            var context = new BattleContext([p1], [p2]);
            var tackleAction = new MoveAction() { Actor = p2, Move = tackle, Context = context };
            tackleAction.Execute(p1);

            var mirror = Utils.GetMove("mirror-move");
            var mirroAction = new MoveAction() {Actor = p1, Move = mirror, Context = context };
            var result = mirroAction.Execute(p2);

            var damage = result.Events.Find(e => e.Type == "damage");
            Assert.NotNull(damage);
            Assert.Contains("usó", damage!.Message);

            Assert.True(result.LastDamage > 0);

           
        }
    }
}
