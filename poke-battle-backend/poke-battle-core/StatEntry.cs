namespace poke.battle.core 
{
    public class StatEntry 
    {
        public Stat  Stat { get; }
        public int BaseValue { get; set;}
        public int  IV { get; }
        public int EV { get; set; }
        public int RawValue { get; set; }
        public int Modifier {get; set; } = 0;
        public double ModValue 
        {
            get 
            {
                if(Modifier > 0)
                    return RawValue * Calculator.GetModMultiplier(Modifier);
                else if(Modifier < 0) 
                    return RawValue * Calculator.GetModMultiplier(Modifier);
                else
                    return RawValue;
            }
        }

        public void ChangeModifier(int delta) =>  Modifier = Math.Clamp(Modifier + delta, -6, 6);
        public void Reset() => Modifier = 0;
        public StatEntry(Stat stat, int baseValue) {
            this.Stat = stat;
            this.BaseValue = baseValue;
            Random rng = new Random();
            IV = new Random().Next(0, 32);
        }
    }
}