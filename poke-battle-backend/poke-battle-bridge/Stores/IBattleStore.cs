using poke.battle.core;

namespace poke.battle.bridge.Stores
{
    public interface IBattleStore
    {
        Battle? GetBattleById(string battleId);
        void Update(string battleId, Battle updated);
    }
}
