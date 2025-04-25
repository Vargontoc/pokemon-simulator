namespace poke_battle_api.dtos
{
    public class PBattlerDto
    {
        public string Nickname { get; set; } = string.Empty;
        public int Level { get; set; }
        public int CurrentHP { get; set; }
        public TypeDto[] Types { get; set; } = [];
        public string[] Moves { get; set; } = [];
        public object[] Stats { get; set; } = {};
    }
}
