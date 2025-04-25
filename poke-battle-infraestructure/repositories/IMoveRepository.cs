
using poke.battle.infraestructure.filters;
using poke.battle.Models;

namespace poke.battle.infraestructure.repositories
{
    public interface IMoveRepository : IRepository<MoveModel, MovesFilter> {

        void SaveAll(List<MoveModel> models);
    }
}
