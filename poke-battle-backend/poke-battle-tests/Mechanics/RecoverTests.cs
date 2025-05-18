using poke.battle.core;
using poke.battle.factories;
using poke.battle.services.impl;
using poke.battle.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using poke.battle.Models.helpers;

namespace poke_battle_tests.Mechanics
{
    public class RecoverTests
    {
        [Fact]
        public void Recover_Should_Heal()
        {
            var attacker = Utils.GetBattler("bulbasaur", 50);
            attacker.IsActive = true;
            attacker.CurrentHp = 20;

            var defender = Utils.GetBattler("charmander", 50);
            defender.IsActive = true;

            var protect = Utils.GetMove("recover");
            protect.Move.MoveEffect = MoveEffect.Recover;

            var context = new BattleContext([attacker], [defender]);
            var action = new MoveAction() { Actor = attacker, Move = protect, Context = context };
             action.Execute(defender);

            int maxHP = attacker.MaxHP;
            int expected = Math.Min(attacker.MaxHP, 20 + (attacker.MaxHP / 2));
            Assert.Equal(expected, attacker.CurrentHp);
        }
    }
}
