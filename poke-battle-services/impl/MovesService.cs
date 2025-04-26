using poke.battle.infraestructure.filters;
using poke.battle.infraestructure.repositories;
using poke.battle.infraestructure.repositories.impl;
using poke.battle.Models;
using poke.battle.services;

namespace poke.battle.services.impl
{
    public class MovesService(IMoveRepository repository) : IMoveService
    {

        public PageResponse<MoveModel> FindAll(MovesFilter filter, PageRequest pageRequest)
        {
            return repository.GetAll(filter, pageRequest);
        }

        public PageResponse<MoveModel> GetAll(MovesFilter filter, PageRequest request) 
        {
            return repository.GetAll(filter, request);
        }

        public MoveModel GetById(int id)
        {
            return repository.FindById(id);
        }

        public MoveModel GetByName(string name)
        {
            return repository.FindByName(name);
        }
    }
}