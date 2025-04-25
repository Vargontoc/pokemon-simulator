namespace poke.battle.core
{
    public class BattleEvent {
        public string Type { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public object? PayLoad { get; set;}
    }
}