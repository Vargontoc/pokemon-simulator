using poke.battle.infraestructure.filters;
using poke.battle.Models.Impl;

namespace poke.battle.infraestructure.repositories
{
    public interface ITypeRepository : IRepository<TypeModel, TypesFilter> {}
}