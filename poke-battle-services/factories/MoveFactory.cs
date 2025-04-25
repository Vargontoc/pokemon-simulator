using poke.battle.core;
using poke.battle.services;

namespace poke_battle_services.factories
{
    public  class MoveFactory
    {
        private readonly IMoveService moveService;
        public MoveFactory(IMoveService moveService )
        {
            this.moveService = moveService;
        }

        public PBattleMove Create(string name)
        {
            var move = moveService.GetByName(name);
            if (move == null)
                throw new Exception($"Move '{name}' not found");

            return new PBattleMove(move);
        }
    }
}
