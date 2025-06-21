using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace poke_battle_infraestructure.filters
{
    public enum FilterType
    {
        equals, notEquals, greaterThan, lessEqual, greaterThanOrEqual, lessThanOrEqual, contains, startsWith, endsWith, inList, notInList, isNull, isNotNull
    }
}
