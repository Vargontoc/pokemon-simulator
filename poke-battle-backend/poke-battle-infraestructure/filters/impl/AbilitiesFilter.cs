using poke.battle.Models.Impl;
using poke_battle_infraestructure.filters;

namespace poke.battle.infraestructure.filters.impl
{
    public class AbilitiesFilter : IFilter<AbilityModel>
    {
        public List<FilterEntry> Entries { get; set; } = []; 
    }
}