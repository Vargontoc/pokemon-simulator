namespace poke_battle_api.dtos
{
    public class CalcExpDto
    {
        public string Code { get; set; } = string.Empty;
        public int Level { get; set; }
        public int BaseExperience { get; set; }
        public int Participants { get; set; } = 1;
        public bool Trainer { get; set; } = false;
        public bool LuckyEgg { get; set; } = false;

    }
}
