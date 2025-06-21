using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using poke.battle.infraestructure.filters;
using poke.battle.Models.Impl;

namespace poke.battle.infraestructure.repositories.impl
{
    public class TypesRepository : GenericRepository<TypeModel, TypesFilter>, ITypeRepository 
    {
    
        public TypesRepository(): base() {

            STORED_FILE = "types.json";
        }

        protected override void CreatePredicates(TypesFilter filter)
        {
            // Empty method
        }
    } 
}