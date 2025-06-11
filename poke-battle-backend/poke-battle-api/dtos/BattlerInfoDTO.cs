namespace poke_battle_api.dtos
{
    public class BattlerInfoDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty ;
        public int Hp { get; set; }
        public int Level { get; set; } = 0;
        public int MaxHp { get; set; }
        public bool IsActive { get; set; }
        public bool IsPlayer { get; set; }
        public bool WasOnBattle { get; set; }
        public int HPPerentage => (int)((double)Hp / MaxHp * 100);
        public object[] Stats { get; set; }  = { };
        public object[] Types { get; set; } = { };
        public List<BattlerMoveInfo> Moves { get; set; } = [];
    }
}
