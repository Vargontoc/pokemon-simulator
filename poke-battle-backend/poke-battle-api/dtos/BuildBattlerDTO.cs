using Microsoft.Extensions.Primitives;

namespace poke_battle_api.dtos
{
    public class BuildBattlerDTO
    {
        public string Specie { get; set; } = string.Empty;
        public int Level { get; set; } = 0;
        public string Nature { get; set; } = string.Empty;
        public string Ability {  get; set; } = string.Empty;
        public string Item { get; set; } = string.Empty;
        public Tuple<int, int>? Ivs { get; set; }
        public Tuple<int, int?>?  Evs { get; set; }
        public string[] Moves { get; set; } = [];
    }
}
