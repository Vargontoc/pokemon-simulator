using System.Runtime.CompilerServices;

namespace poke.battle.core 
{
    public sealed class Stat 
    {
        /// <summary>
        /// Obtiene el nombre del stat
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Obtiene la abreviatura del stat
        /// </summary>
        public string Abbr {get; private set; }
        /// <summary>
        /// Obtiene el códdigo interno del stat
        /// </summary>
        public string Code { get; private set; }
        public byte Index { get; private set; }
        Stat(String code, string name, string abbr, byte index = 0)
        {
            this.Code = code;
            this.Name = name;
            this.Abbr = abbr;
            Index = index;
        }
        public static readonly Stat Hp = new("hp", "PS", "PS", 0);
        public static readonly Stat Attk = new("atk","Ataque", "At.", 1);
        public static readonly Stat Def = new("def","Defensa", "Def.", 2);
        public static readonly Stat AtkSp = new("SpAtk","Ataque Especial", "At.Esp.", 3);
        public static readonly Stat DefSp = new("SpDef","Defensa Especial", "Def. Esp.", 4);
        public static readonly Stat Spd = new("spd","Velocidad", "Vel.", 5);
        
    public static IEnumerable<Stat> GetValues() {
        yield return Hp;
        yield return Attk;
        yield return Def;
        yield return AtkSp;
        yield return DefSp;
        yield return Spd;
    }

    public override bool Equals(object? obj)
    {

        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
            
        var s = (Stat)obj;
        return s.Name.Equals(s.Name);
    }
        
    public override int GetHashCode()
    {

        return Name.GetHashCode();
    }

    }
}