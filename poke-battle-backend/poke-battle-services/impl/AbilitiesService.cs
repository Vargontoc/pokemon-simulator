
using poke.battle.infraestructure.filters;
using poke.battle.infraestructure.filters.impl;
using poke.battle.infraestructure.repositories;
using poke.battle.Models.Impl;
using poke_battle_infraestructure.validators;

namespace poke.battle.services.impl
{
    public class AbilitiesService(IAbilitiesRespository repository, IPokemonService specieSerice, Validator<AbilityModel> validator) : IAbilityService
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
            validator.Validate(model);
            return repository.Save(model);
        }

        public AbilityModel Update(AbilityModel model)
        {
            validator.Validate(model);
            return repository.Update(model);
        }

        public bool Delete(int id)
        {
            var toDelete = GetById(id);
            var species = specieSerice.FindAll(null!, null!).Results;
            if (species.Any(x => (x.Abilities != null && x.Abilities.Contains(toDelete.Name)) || (toDelete.Name.Equals(x.HiddenAbility))))
                throw new RepositoryException($"La habilidad '{toDelete.DisplayName}' la tiene asignada algun Pokémon");

            return repository.Delete(toDelete);

        }
    }
}