using poke.battle.infraestructure.filters;
using poke.battle.Models.Impl;

namespace poke.battle.services
{
    public interface ITypeService : IGenericService<TypeModel, TypesFilter>
    {
        TypeModel Save(TypeModel model);
        TypeModel Update(TypeModel model);
    }
}