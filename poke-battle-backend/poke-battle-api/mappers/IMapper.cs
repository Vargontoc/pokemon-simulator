using poke.battle.core;
using poke.battle.Models;

namespace poke_battle_api.mappers
{
    /// <summary>
    /// Mapeador de objetos
    /// </summary>
    /// <typeparam name="T">Objetos que va a pasar a Front-End</typeparam>
    /// <typeparam name="M">Modelo de datos</typeparam>
    public interface IMapper<T, M> where T : class
        where M : IModel
    {
        /// <summary>
        /// Convierte un objeto de Front-End a modelo de datos
        /// </summary>
        /// <param name="model">Objeto de Front-End</param>
        /// <returns></returns>
        M convertToBack(T model);
        /// <summary>
        /// Convierte un modelo de datos a objeto de Front-End
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        T convertToFront(M model);
        /// <summary>
        /// Convierte una lista de objetos a modelos de datos
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        IEnumerable<M> convertToBack(IEnumerable<T> list);
        /// <summary>
        /// Convierte una lista de modelo de datos a objetos Front-End
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        IEnumerable<T> convertToFront(IEnumerable<M> list);
    }
}
