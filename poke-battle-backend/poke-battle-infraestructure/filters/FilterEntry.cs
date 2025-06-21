using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace poke_battle_infraestructure.filters
{
    public class FilterEntry
    {
        public FilterType Type { get; set; } = FilterType.contains;
        public string Property { get; set; } = string.Empty;
        public object? Value { get; set; } = null;
    }
}
