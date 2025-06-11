namespace poke_battle_api.dtos
{
    public class BattlerMoveInfo
    {
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public object? Type { get; set; }
        public object? Category { get; set; }
        public int PP { get; set; }
        public int MaxPP { get; set; }
        public int? Power { get; set; }
        public int? Accuracy { get; set; }
        public int Priority { get; set; } = 0;
        public string Effect { get; set; } = string.Empty;

    }
}
