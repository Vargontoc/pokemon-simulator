using poke.battle.Models;

namespace poke.battle.infraestructure.filters {
    public class MovesFilter : IFilter<MoveModel>
    {
        public string Search { get; set; } = string.Empty;
    }
}