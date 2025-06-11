namespace poke.battle.bridge.api
{
    public class BattleDecisionResponse
    {
        public string Action  { get; set; } = "move";
        public string Value   { get; set; } = string.Empty;
        public string Rationale { get; set; } = string.Empty;   
    }
}
