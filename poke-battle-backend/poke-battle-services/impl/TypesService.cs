using poke.battle.infraestructure.filters;
using poke.battle.infraestructure.repositories;
using poke.battle.infraestructure.repositories.impl;
using poke.battle.Models.Impl;
using poke_battle_infraestructure.validators;

namespace poke.battle.services.impl
{
    public class TypesService(ITypeRepository repository) : ITypeService
    {

        public PageResponse<TypeModel> FindAll(TypesFilter filter, PageRequest pageRequest)
        {
            return repository.GetAll(filter, pageRequest);
        }

        public PageResponse<TypeModel> GetAll(TypesFilter filter, PageRequest request) 
        {
            return repository.GetAll(filter, request);
        }

        public TypeModel GetById(int id)
        {
            return repository.FindById(id);
        }

        public TypeModel GetByName(string name)
        {
            return repository.FindByName(name);
        }

        public TypeModel Save(TypeModel model)
        {
            ValidationHelper.Validate(v =>
            {
                v.Check(string.IsNullOrEmpty(model.Name), "El nombre interno es obligatorio");
                v.Check(string.IsNullOrEmpty(model.DisplayName), "El nombre público es obligatorio");
                v.Check(!string.IsNullOrEmpty(model.Name) && repository.FindByName(model.Name) != null, "Ya existe un tipo con ese nombre");
            });

            return repository.Save(model);
        }

        public TypeModel Update(TypeModel model)
        {
            return repository.Update(model);
        }
    }
}