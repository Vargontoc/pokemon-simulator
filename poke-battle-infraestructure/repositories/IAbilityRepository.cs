using poke.battle.infraestructure.filters.impl;
using poke.battle.Models.Impl;

namespace poke.battle.infraestructure.repositories
{
    public interface IAbilitiesRespository : IRepository<AbilityModel, AbilitiesFilter> {

        void SaveAll(List<AbilityModel> abilities);
     }
}