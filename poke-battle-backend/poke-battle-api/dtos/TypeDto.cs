using poke_battle_api.utils;

namespace poke_battle_api.dtos
{
    public class TypeDto
    {
        public int Id { get; set; }
        public string InternalName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public Combo Weakness { get; set; } = new();
        public Combo Resistences { get; set; } = new();
        public Combo Inmunities { get; set; } = new();
        public string Icon { get; set; } = string.Empty; 

    }
}
