using System.Runtime.CompilerServices;

namespace poke.battle.core {
    public sealed class Nature
    {
        public string Name { get; private set;}
        public Stat Plus {get; private set;}
        public Stat Minus { get; private set;}
        Nature( string name, Stat? plus = null, Stat? minus = null)
        {
            this.Name = name;
            this.Plus = plus!;
            this.Minus = minus!;
        }

        public double GetModifier(Stat check) 
        {
            return Plus == check ? 1.1 : Minus == check ? 0.9 : 1.0;
        }
        public static readonly Nature Adamant   = new("Firme", Stat.Attk, Stat.AtkSp);
        public static readonly Nature Bashful   = new("Ingenua"); // neutra
        public static readonly Nature Bold      = new("Osada", Stat.Def, Stat.Attk);
        public static readonly Nature Brave     = new("Audaz", Stat.Attk, Stat.Spd);
        public static readonly Nature Calm      = new("Serena", Stat.DefSp, Stat.Attk);
        public static readonly Nature Careful   = new("Cauta", Stat.DefSp, Stat.AtkSp);
        public static readonly Nature Docile    = new("Dócil"); // neutra
        public static readonly Nature Gentle    = new("Amable", Stat.DefSp, Stat.Def);
        public static readonly Nature Hardy     = new("Fuerte"); // neutra
        public static readonly Nature Hasty     = new("Activa", Stat.Spd, Stat.Def);
        public static readonly Nature Impish    = new("Agitada", Stat.Def, Stat.AtkSp);
        public static readonly Nature Jolly     = new("Alegre", Stat.Spd, Stat.AtkSp);
        public static readonly Nature Lax       = new("Floja", Stat.Def, Stat.DefSp);
        public static readonly Nature Lonely    = new("Furiosa", Stat.Attk, Stat.Def);
        public static readonly Nature Mild      = new("Afable", Stat.AtkSp, Stat.Def);
        public static readonly Nature Modest    = new("Modesta", Stat.AtkSp, Stat.Attk);
        public static readonly Nature Naive     = new("Ingenua", Stat.Spd, Stat.DefSp);
        public static readonly Nature Naughty   = new("Pícara", Stat.Attk, Stat.DefSp);
        public static readonly Nature Quiet     = new("Plácida", Stat.AtkSp, Stat.Spd);
        public static readonly Nature Quirky    = new("Rara"); // neutra
        public static readonly Nature Rash      = new("Alocada", Stat.AtkSp, Stat.DefSp);
        public static readonly Nature Relaxed   = new("Plácida", Stat.Def, Stat.Spd);
        public static readonly Nature Sassy     = new("Grosera", Stat.DefSp, Stat.Spd);
        public static readonly Nature Serious   = new("Seria"); // neutra
        public static readonly Nature Timid     = new("Miedosa", Stat.Spd, Stat.Attk);

        public static IEnumerable<Nature> GetValues() 
        {
            yield return Hardy;
            yield return Lonely;
            yield return Brave;
            yield return Adamant;
            yield return Naughty;

            yield return Bold;
            yield return Docile;
            yield return Relaxed;
            yield return Impish;
            yield return Lax;

            yield return Timid;
            yield return Hasty;
            yield return Serious;
            yield return Jolly;
            yield return Naive;

            yield return Modest;
            yield return Mild;
            yield return Quiet;
            yield return Bashful;
            yield return Rash;

            yield return Calm;
            yield return Gentle;
            yield return Sassy;
            yield return Careful;
            yield return Quirky;
        }

        public static Nature Random()
        {
            Random rng = new();
            var values = GetValues().ToList();
            return values[rng.Next(values.Count)];
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            
            var n = (Nature)obj;

            return this.Name.Equals(n.Name) && this.Plus.Equals(n.Plus) & this.Minus.Equals(n.Minus);
        }
        
        // override object.GetHashCode
        public override int GetHashCode()
        {
            return  Name.GetHashCode() ^ Plus.GetHashCode() ^ Minus.GetHashCode();
        }
    }
}