using poke.battle.infraestructure.filters;
using poke.battle.Models.Impl;

namespace poke.battle.infraestructure.repositories
{
    public interface IPokemonsRepository : IRepository<SpecieModel, PokemonsFilter> {

        void SaveAll(List<SpecieModel> species);
    }
}