using System.Net.Http.Headers;
using Newtonsoft.Json;
using poke.battle.infraestructure.filters.impl;
using poke.battle.Models.Impl;

namespace poke.battle.infraestructure.repositories.impl
{
    public class AbilitiesRepository : GenericRepository<AbilityModel, AbilitiesFilter>, IAbilitiesRespository
    {

        public AbilitiesRepository():base()
        {
            STORED_FILE = "abilities.json";
        }


        public void SaveAll(List<AbilityModel> abilities)
        {
            Save(abilities);
        }

        protected override void CreatePredicates(AbilitiesFilter filter)
        {
            // Empty method
        }
    }
}