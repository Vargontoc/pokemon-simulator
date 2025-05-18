using poke.battle.infraestructure.filters;
using poke.battle.Models;

namespace poke.battle.infraestructure.repositories {
    /// <summary>
    /// Interfaz que representa un repositorio de datos
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T, F>
     where T: IModel
     where F: IFilter<T>
    {
        
        PageResponse<T> GetAll(F filter, PageRequest pageRequest);

        T FindByName(string name);

        T FindById(int id);

        T Save(T entity);

        T Update(T entity);

        bool Delete(T entity);
    }
}
