using System.Data;
using System.Runtime.InteropServices.Marshalling;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using poke.battle.infraestructure.filters;
using poke.battle.Models.Impl;

namespace poke.battle.infraestructure.repositories.impl
{
    public class PokemonsRepository : GenericRepository<SpecieModel, PokemonsFilter>, IPokemonsRepository
    {
        public PokemonsRepository():base()
        {
            STORED_FILE = "pokemons.json";
        }

        protected override void CreatePredicates(PokemonsFilter filter)
        {
            // Empty method
        }

        public void SaveAll(List<SpecieModel> species)
        {
            Save(species);
        }
    }
}