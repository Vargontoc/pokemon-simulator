using System.Runtime.CompilerServices;
using poke.battle.Models;
using poke_battle_infraestructure.filters;

namespace poke.battle.infraestructure.filters
{
    public interface IFilter<T> where T : IModel 
    {
        List<FilterEntry> Entries { get; set; }
    }
}