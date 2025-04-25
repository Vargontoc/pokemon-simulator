using poke.battle.infraestructure.filters;
using poke.battle.Models.Impl;

namespace poke.battle.services
{
    public interface IPokemonService : IGenericService<SpecieModel, PokemonsFilter>
    {
    }
}