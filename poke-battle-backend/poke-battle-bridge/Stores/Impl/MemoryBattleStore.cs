using poke.battle.core;

namespace poke.battle.bridge.Stores.Impl
{
    public class MemoryBattleStore : IBattleStore
    {
        private readonly Dictionary<string, Battle> _battles = [];
        public Battle? GetBattleById(string battleId) => _battles.TryGetValue(battleId, out var battle) ? battle : null;

        public void Update(string battleId, Battle updated) => _battles[battleId] = updated;

        public void AddMockBattle(PBattler player, PBattler rival)
        {
            if(!_battles.ContainsKey("mock-battle"))
            {
                Battle b = new Battle(new BattleContext(player: [player], rival: [rival]));
            }
        }
    }
}
