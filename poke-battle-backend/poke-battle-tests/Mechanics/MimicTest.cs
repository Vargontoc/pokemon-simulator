using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using poke.battle.core;
using poke.battle.Models.helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace poke_battle_tests.Mechanics
{
    public  class MimicTest
    {
        [Fact]
        public void Mimic_ShouldCopy()
        {
            var attacker = Utils.GetBattler("bulbasaur", 50);
            attacker.IsActive = true;

            var mimic = Utils.GetMove("mimic");
            mimic.Move.MoveEffect = MoveEffect.Mimic;
            attacker.SetMoves([mimic]);


            var defender = Utils.GetBattler("charmander", 50);
            defender.IsActive = true;

            var tackle = Utils.GetMove("tackle");
            var growl = Utils.GetMove("growl");
            defender.SetMoves([tackle, growl]);


            var context = new BattleContext([attacker], [defender]);
            var action = new MoveAction() { Actor = attacker, Context = context, Move = mimic };

            var result = action.Execute(defender);

            Assert.DoesNotContain(attacker.Moves, m => m.Move.Name == "mimic");
            Assert.Contains(attacker.Moves, m => m.Move.Name == "tackle" || m.Move.Name == "growl");
            Assert.Contains(result.Events, e => e.Type == "copy");
        }

        [Fact]
        public void Mimic_FailIfNoNewMove()
        {
            var attacker = Utils.GetBattler("bulbasaur", 50);
            attacker.IsActive = true;

            var tackle = Utils.GetMove("tackle");
            var mimic = Utils.GetMove("mimic");
            mimic.Move.MoveEffect = MoveEffect.Mimic;
            attacker.SetMoves([mimic, tackle]);
            

            var defender = Utils.GetBattler("charmander", 50);
            defender.IsActive = true;

            var growl = Utils.GetMove("growl");
            defender.SetMoves([tackle]);


            var context = new BattleContext([attacker], [defender]);
            var action = new MoveAction() { Actor = attacker, Context = context, Move = mimic };
            var result  = action.Execute(defender);

            Assert.Contains(attacker.Moves, m => m.Move.Name == "mimic");
            Assert.Contains(result.Events, e => e.Type == "fail" || e.Type == "copy-fail");
        }
    }

}
