using poke.battle.Models;
using poke_battle_infraestructure.filters;

namespace poke.battle.infraestructure.filters {
    public class MovesFilter : IFilter<MoveModel>
    {
        public List<FilterEntry> Entries { get; set; } = [];
    }
}