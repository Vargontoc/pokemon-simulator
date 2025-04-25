namespace poke_battle_api.utils
{
    public class ComboItem
    {
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public bool Checked { get; set; }

        // Optional
        public string? Icon { get; set; }
        public string? Tooltip { get; set; }
    }
}
