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
            if(!string.IsNullOrEmpty(filter.WeakTo))
            {
                predicates.Add(x => x.Weakness.Contains(filter.WeakTo));
            }
            if (!string.IsNullOrEmpty(filter.ResistenceTo))
            {
                predicates.Add(x => x.Resistences.Contains(filter.ResistenceTo));
            }
            if (!string.IsNullOrEmpty(filter.InmunityTo))
            {
                predicates.Add(x => x.Inmunities.Contains(filter.InmunityTo));
            }
        }
    } 
}