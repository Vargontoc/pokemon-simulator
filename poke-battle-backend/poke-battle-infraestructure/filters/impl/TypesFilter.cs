using poke.battle.Models.Impl;
using poke_battle_infraestructure.filters;

namespace poke.battle.infraestructure.filters
{
    public class TypesFilter : IFilter<TypeModel>
    {
        public List<FilterEntry> Entries { get; set; } = [];

    }
}