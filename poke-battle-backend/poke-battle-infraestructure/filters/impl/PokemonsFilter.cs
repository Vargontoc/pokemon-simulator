using poke.battle.Models.Impl;
using poke_battle_infraestructure.filters;

namespace poke.battle.infraestructure.filters
{
    public class PokemonsFilter : IFilter<SpecieModel>
    {
        public List<FilterEntry> Entries { get; set; } = [];
  
    }
}