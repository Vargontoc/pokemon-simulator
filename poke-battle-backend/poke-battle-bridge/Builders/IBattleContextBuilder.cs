using poke.battle.core;

namespace poke.battle.bridge.Builders
{
    public interface IBattleContextBuilder
    {
        BattleContext BuildFromBattle(Battle battle);
    }
}
