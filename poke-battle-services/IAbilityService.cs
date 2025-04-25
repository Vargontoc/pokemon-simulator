using poke.battle.infraestructure.filters.impl;
using poke.battle.Models.Impl;

namespace poke.battle.services
{
    public interface IAbilityService : IGenericService<AbilityModel, AbilitiesFilter>
    {
        AbilityModel Save(AbilityModel model);
    }
}