using poke.battle.infraestructure.filters;
using poke.battle.Models;

namespace poke.battle.services
{
    public interface IMoveService : IGenericService<MoveModel, MovesFilter>
    {
    }
}