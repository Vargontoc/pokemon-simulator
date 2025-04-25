using poke.battle.infraestructure.filters;

namespace poke.battle.api.utils {
    public class QueryRequest<T> 
    {
        public T Filter { get; set;} = default(T)!;
        public PageRequest Request {get; set;} = null!;
    }
}