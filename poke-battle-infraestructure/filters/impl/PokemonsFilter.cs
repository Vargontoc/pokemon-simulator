using poke.battle.Models.Impl;

namespace poke.battle.infraestructure.filters
{
    public class PokemonsFilter : IFilter<SpecieModel>
    {
        public string Search { get; set; } = string.Empty;
    }
}