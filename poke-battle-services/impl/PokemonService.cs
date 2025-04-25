using poke.battle.infraestructure.filters;
using poke.battle.infraestructure.repositories;
using poke.battle.infraestructure.repositories.impl;
using poke.battle.Models.Impl;

namespace poke.battle.services.impl
{
    public class PokemonsService(HttpClient client) : IPokemonService
    {
        private readonly IPokemonsRepository repository = new PokemonsRepository(client);

        public PageResponse<SpecieModel> FindAll(PokemonsFilter filter, PageRequest pageRequest)
        {
            return repository.GetAll(filter, pageRequest);
        }

        public SpecieModel GetById(int id)
        {
            return repository.FindById(id);
        }

        public SpecieModel GetByName(string name)
        {
            return repository.FindByName(name);
        }
    }

}