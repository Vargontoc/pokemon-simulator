
using poke.battle.infraestructure.filters;
using poke.battle.infraestructure.filters.impl;
using poke.battle.infraestructure.repositories;
using poke.battle.Models.Impl;
using poke_battle_infraestructure.validators;

namespace poke.battle.services.impl
{
    public class AbilitiesService(IAbilitiesRespository repository) : IAbilityService
    {

        public PageResponse<AbilityModel> FindAll(AbilitiesFilter filter, PageRequest pageRequest)
        {
            return repository.GetAll(filter, pageRequest);
        }


        public AbilityModel GetById(int id)
        {
            return repository.FindById(id);
        }

        public AbilityModel GetByName(string name)
        {
            return repository.FindByName(name);
        }

        public AbilityModel Save(AbilityModel model)
        {
            ValidationHelper.Validate(v =>
            {
                v.Check(string.IsNullOrEmpty(model.Name), "El nombre interno es obligatorio");
                v.Check(string.IsNullOrEmpty(model.DisplayName), "El nombre público es obligatorio");
                v.Check(!string.IsNullOrEmpty(model.Name) && repository.FindByName(model.Name) != null, "Ya existe una habilidad con ese nombre");
            });

            return repository.Save(model);
        }
    }
}