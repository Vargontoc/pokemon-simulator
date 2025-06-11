using poke.battle.core;

namespace poke.battle.bridge.Builders.Impl
{
    public class BattleContextBuilder : IBattleContextBuilder
    {
        public BattleContext BuildFromBattle(Battle battle)
        {
            return battle.Context;
        }
    }
}
