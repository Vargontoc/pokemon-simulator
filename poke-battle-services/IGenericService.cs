using poke.battle.infraestructure.filters;
using poke.battle.Models;

namespace poke.battle.services
{
    public interface IGenericService<T, F>
        where T: IModel
        where F: IFilter<T>
    {
        T GetByName(string name);
        T GetById(int id);
        PageResponse<T> FindAll(F filter, PageRequest pageRequest);
        
    }
}