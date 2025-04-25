using poke.battle.core;
using poke.battle.factories;
using poke.battle.Models.helpers;
using poke.battle.services.impl;
using poke.battle.services;

namespace poke_battle_tests.Mechanics;

public class Protect
{
    [Fact]
    public void Test1()
    {

        var attacker = Utils.GetBattler("bulbasaur", 50);
        attacker.IsActive = true;
        var defender = Utils.GetBattler("charmander", 50);
        defender.IsActive = true;

        var protect = Utils.GetMove("protect");
        protect.Move.MoveEffect = MoveEffect.Protect;
       
        var tackle = Utils.GetMove("tackle");

        var context = new BattleContext([attacker], [defender]);
        var actions = new List<PlayerAction>
            {
                new MoveAction() { Actor = attacker, Move = tackle, Context = context },
                new MoveAction() { Actor = defender, Move = protect, Context = context }
            };

        var turn = new BattleTurn(context, actions, 1);
        turn.ExecuteTurn();

        var hpAfter = defender.CurrentHp;
        var protectedEvent = turn.Events.FirstOrDefault(e => e.Type == "protect-blocked");

        Assert.NotNull(protectedEvent);
        Assert.Equal(defender.GetStat(Stat.Hp).RawValue, hpAfter);
    }
}
