namespace poke_battle_api.dtos
{
    public class BattlePlayerRequest
    {
        public enum BattleTypeRequest { Attack, Switch, Surronder }

        public BattleTypeRequest Action { get; set; }
        public string Value { get; set; } = string.Empty;
    }
}
