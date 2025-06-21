using poke.battle.infraestructure.filters;
using poke.battle.infraestructure.repositories;
using poke.battle.infraestructure.repositories.impl;
using poke.battle.Models.Impl;
using poke_battle_infraestructure.validators;

namespace poke.battle.services.impl
{
    public class TypesService(ITypeRepository repository, IPokemonService speciesService, IMoveService movesService, Validator<TypeModel> validator) : ITypeService
    {

        public PageResponse<TypeModel> FindAll(TypesFilter filter, PageRequest pageRequest)
        {
            return repository.GetAll(filter, pageRequest);
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
            validator.Validate(model);
            var availableTypes = repository.GetAll(null!, null!).Results.Select(x => x.Name).ToArray();

            if (!IsValidSelected(model.Weakness, availableTypes))
                throw new ValidationException(["Algun tipo seleccionado en debilidades no existe"]);

            if (!IsValidSelected(model.Resistences, availableTypes))
                throw new ValidationException(["Algun tipo seleccionado en resistencias no existe"]);
           
            if (!IsValidSelected(model.Inmunities, availableTypes))
                throw new ValidationException(["Algun tipo seleccionado en inmunidades no existe"]);
            
            
            return repository.Save(model);
        }

        public TypeModel Update(TypeModel model)
        {
            validator.Validate(model);
            var availableTypes = repository.GetAll(null!, null!).Results.Select(x => x.Name).ToArray();

            if (!IsValidSelected(model.Weakness, availableTypes))
                throw new ValidationException(["Algun tipo seleccionado en debilidades no existe"]);

            if (!IsValidSelected(model.Resistences, availableTypes))
                throw new ValidationException(["Algun tipo seleccionado en resistencias no existe"]);
            
            if (!IsValidSelected(model.Inmunities, availableTypes))
                throw new ValidationException(["Algun tipo seleccionado en inmunidades no existe"]);


           return repository.Update(model);
        }

        public bool Delete(int id)
        {
            var toDelete = GetById(id);
            var moves = movesService.FindAll(null!, null!).Results;
            if (moves.Any(x => (x.Type != null && x.Type == toDelete.Name)))
                throw new RepositoryException($"El tipo '{toDelete.DisplayName}' esta en uso en algun movimiento");

            var species = speciesService.FindAll(null!, null!).Results;
            if(species.Any(x => x.Types != null && x.Types.Contains(toDelete.Name)))
                throw new RepositoryException($"El tipo '{toDelete.DisplayName}' está en uso en alguna especie Pokémon");

            var storedTypes = repository.GetAll(null!, null!).Results;
            foreach(TypeModel t in storedTypes)
            {
                if (t.Name == toDelete.Name)
                    continue;
                t.Weakness.ToList().Remove(toDelete.Name);
                t.Resistences.ToList().Remove(toDelete.Name);
                t.Inmunities.ToList().Remove(toDelete.Name);

                repository.Update(t);
            }
            

            return repository.Delete(toDelete);
        }

        private static bool IsValidSelected(string[] selected, string[] stored)
        {
            if (selected == null || selected.Length == 0) return true;
            foreach(string s in selected)
            {
                if (!stored.Contains(s))
                {
                    return false;
                }
            }

            return true;
        }

    }
}