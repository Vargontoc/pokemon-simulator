using poke.battle.Models.Impl;

namespace poke.battle.infraestructure.filters.impl
{
    public class AbilitiesFilter : IFilter<AbilityModel>
    {
        public string Search { get; set;} = string.Empty;
    }
}