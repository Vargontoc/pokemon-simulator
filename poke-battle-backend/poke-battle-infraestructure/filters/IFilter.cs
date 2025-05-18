using System.Runtime.CompilerServices;
using poke.battle.Models;

namespace poke.battle.infraestructure.filters
{
    public interface IFilter<T> where T : IModel 
    {
        string Search { get; set; }
    }
}