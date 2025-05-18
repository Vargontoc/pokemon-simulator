using Newtonsoft.Json;
using poke.battle.Models.converters;

namespace poke.battle.Models.Impl {
    public class SpecieModel : IModel
    {
        /// <summary>
        /// Obtiene o establece nombre interno de la es`pecie
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Obtiene o establece el identificador de la especie
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Obtiene el nombre de la especie
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;
        /// <summary>
        /// Obtiene o establece la Pokédex
        /// </summary>
        public string Pokedex { get; set; } = string.Empty;
        /// <summary>
        /// Obtiene o establece la felicidad base de la especie
        /// </summary>
        public int Happiness {get; set; }
        /// <summary>
        /// Obtiene o establece los stats base de la especie
        /// </summary>
        public int[] StatsBase { get; set; } = [];
        /// <summary>
        /// Obtiene o establece los puntos de esfuerzo
        /// </summary>
        public int[] Efforts {get; set; } = [];
        /// <summary>
        /// Obtiene o establece los tipos del pokemon
        /// </summary>
        public string[] Types { get; set; } = [];
        /// <summary>
        /// Obtiene o establece las habilidades que puede tener la especie
        /// </summary>
        public string[] Abilities { get; set; } = [];
        /// <summary>
        /// Obtiene o establece la habilidad oculta que puede tener la especie
        /// </summary>
        public string HiddenAbility { get; set;} = string.Empty;
        /// <summary>
        /// Obtiene o establece las cadenas de evolucion
        /// </summary>
        public EvolutionChain[] Evolutions { get; set; } = [];
        /// <summary>
        /// Obtiene o establece los movimientos que puede aprender por nivel
        /// </summary>
        public Tuple<int, string>[] Movepool { get; set; } = []; 
        /// <summary>
        /// Obtiene o establece los movimientos huevo
        /// </summary>
        public string[] EggMoves { get; set; } = [];
        /// <summary>
        /// Obtiene o establece los movimientos tutor
        /// </summary>
        public string[] TutorMoves { get; set; } = [];
        /// <summary>
        /// Obtiene o establece los movimientos por mt - mo
        /// </summary>
        public string[] MtMoves {get; set; } = [];
        /// <summary>
        /// Obtiene o establece el grupo huevo que pertenece la especie
        /// </summary>
        [JsonConverter(typeof(EggGroupArrayConverter))]
        public EggGroup[] EggGroups { get; set;} = [];
        /// <summary>
        /// Obtiene o establece el habitat que vive la especie
        /// </summary>
        [JsonConverter(typeof(HabitatConverter))]
        public Habitat Habitat { get;  set; } = Habitat.Unknown;
        /// <summary>
        /// Obtiene o establece el color principal de la especie
        /// </summary>
        [JsonConverter(typeof(ColorConverter))]
        public Color Color { get; set;} = Color.Black;
        /// <summary>
        /// Obtiene o establece la altura media de la especie
        /// </summary>
        public double Height { get; set; }
        /// <summary>
        /// Obtiene o establece el peso medio de la especie
        /// </summary>
        public double Weight { get; set;}
        /// <summary>
        /// Obtiene o establece el ratio de crecimiento de la especie
        /// </summary>
        [JsonConverter(typeof(GrowthConverter))]
        public Growth Growth { get; set; } = Growth.Medium;
        /// <summary>
        /// Obtiene o establece el ratio de captura
        /// </summary>
        public int CaptureRate { get; set; }
        /// <summary>
        /// Obtiene o establece los items que puede tener
        /// </summary>
        public WildItem[] HeldItems {get; set; } = [];
        /// <summary>
        /// Obtiene o establece la experiencia base
        /// </summary>
        public int Exp { get; set; }

    }

    public class EvolutionChain 
    {
        public string Specie { get; set; } = string.Empty;
        [JsonConverter(typeof(EvolutionTriggerConverter))]
        public EvolutionTrigger Trigger { get; set; } = EvolutionTrigger.LevelUp;
        public EvolutionRequirement[] Requirements  { get; set; } = [];
    }

    public sealed class EvolutionTrigger
    {
        public string Code { get; private set; }
        private EvolutionTrigger(string code) 
        {
            this.Code = code;
        }

        public static readonly EvolutionTrigger LevelUp = new("level-up");
        public static readonly EvolutionTrigger UseItem = new("use-item");

        public static IEnumerable<EvolutionTrigger> GetValues() 
        {
            yield return LevelUp;
            yield return UseItem;
        }

        public static EvolutionTrigger Get(string code)
        {
            if(string.IsNullOrEmpty(code))
                return LevelUp;

            return GetValues().FirstOrDefault(x => x.Code == code) ?? LevelUp;
        }    
    }

    public sealed class EvolutionCondition 
    {
        [JsonIgnore]
        public  string Name { get; private set; }
        public string Code { get; private set; }
        private EvolutionCondition(string code, string name) 
        {
            this.Name = name;
            this.Code = code;
        }

        public static readonly EvolutionCondition Item = new("use-item", "Usar objeto");
        public static readonly EvolutionCondition HeldItem = new("held-item", "Portar objeto");
        public static readonly EvolutionCondition KnownMove = new("known-move", "Conocer movimiento");
        public static readonly EvolutionCondition KnownMoveType = new("known-move-type", "Conocer tipo movimiento");
        public static readonly EvolutionCondition Level = new("level", "Por nivel");
        public static readonly EvolutionCondition Happiness = new("happiness", "Por felicidad");
        public static readonly EvolutionCondition OverworldRain = new("overworld-rain", "Está lloviendo");
        public static readonly EvolutionCondition TimeOfDay = new("time-of-day", "Momento del dia");
        public static readonly EvolutionCondition RelativeStats = new("relative-stats", "Relacion stats");
        public static IEnumerable<EvolutionCondition> GetValues() 
        {
            yield return Item;
            yield return HeldItem;
            yield return KnownMove;
            yield return KnownMoveType;
            yield return Level;
            yield return Happiness;
            yield return OverworldRain;
            yield return RelativeStats;
            yield return TimeOfDay;
        }

        public static EvolutionCondition Get(string code)
        {
            if(string.IsNullOrEmpty(code))
                return Level;

            return GetValues().FirstOrDefault(x => x.Code == code) ?? Level;
        }    
    }

    public class EvolutionRequirement
    {
        [JsonConverter(typeof(EvolutionConditionConverter))]
        public EvolutionCondition Condition { get; set; } = EvolutionCondition.Level;
        public string Value {get; set;} = string.Empty;
    }

    public class WildItem 
    {
        /// <summary>
        /// Obtiene o establece el nombre interno del objeto que porta
        /// </summary>
        public string Item { get; set; } = string.Empty;
        /// <summary>
        /// Nivel de rareza de tener el objeto
        /// </summary>
        public int Rarity { get; set; } 
    }

    public sealed class EggGroup 
    {
        [JsonIgnore]
        public  string Name { get; private set; }
        public string Code { get; set; }
        private EggGroup(string code, string name) 
        {
            this.Name = name;
            this.Code = code;
        }

        public static readonly EggGroup Monster = new("monster","Monstruo");
        public static readonly EggGroup Water1 = new("water1","Agua 1");
        public static readonly EggGroup Bug = new("bug","Bicho");
        public static readonly EggGroup Flying = new("flying","Volador");
        public static readonly EggGroup Ground = new("ground","Tierra");
        public static readonly EggGroup Fairy = new("fairy","Hada");
        public static readonly EggGroup Plant = new("plant","Planta");
        public static readonly EggGroup Humanshape = new("humanshape","Humanoide");
        public static readonly EggGroup Water3 = new("water3","Agua 3");
        public static readonly EggGroup Mineral = new("mineral","Mineral");
        public static readonly EggGroup Unknown = new("indeterminate","Desconocido");
        public static readonly EggGroup Water2 = new("water2","Agua 2");
        public static readonly EggGroup Ditto = new("ditto","Ditto");
        public static readonly EggGroup Dragon = new("dragon","Dragón");
        public static readonly EggGroup None = new("no-eggs","-");

        public static IEnumerable<EggGroup> GetValues() 
        {
            yield return Monster;
            yield return Water1;
            yield return Bug;
            yield return Flying;
            yield return Ground;
            yield return Fairy;
            yield return Plant;
            yield return Humanshape;
            yield return Water3;
            yield return Mineral;
            yield return Unknown;
            yield return Water2;
            yield return Ditto;
            yield return Dragon;
            yield return None;
        }
        public static EggGroup Get(string code) {
            if(string.IsNullOrEmpty(code))
                return None;
            return GetValues().FirstOrDefault(x => x.Code == code) ?? None;
        }

    }

    public sealed class Color 
    {
        [JsonIgnore]
        public  string Name { get; private set; }
        public string Code {get; set; }
        private Color(string code, string name) 
        {
            this.Name = name;
            this.Code = code;
        }

        public static readonly Color Black = new("black","Negro");
        public static readonly Color Blue = new("blue", "Azúl");
        public static readonly Color Brown = new("brown", "Marrón");
        public static readonly Color Grey = new("grey", "Gris");
        public static readonly Color Green = new("green","Verde");
        public static readonly Color Pink = new("pink","Rosa");
        public static readonly Color Purple = new("purple", "Morado");
        public static readonly Color Red = new("red","Rojo");
        public static readonly Color White = new("white", "Blanco");
        public static readonly Color Yellow = new("yellow", "Amarillo");

        public static IEnumerable<Color> GetValues () {
            yield return Black;
            yield return Blue;
            yield return Brown;
            yield return Grey;
            yield return Green;
            yield return Pink;
            yield return Purple;
            yield return Red;
            yield return White;
            yield return Yellow;
        }

        
        public static Color Get(string code) {
            if(string.IsNullOrEmpty(code))
                return White;
            return GetValues().FirstOrDefault(x => x.Code == code) ?? White;
        }
    }

    public sealed class Habitat 
    {
        [JsonIgnore]
        public  string Name { get; private set; }
        public string Code { get; set; }
        private Habitat(string code, string name) {
            this.Name = name;
            this.Code = code;
        }

        public static readonly Habitat Cave = new("cave","Cueva");
        public static readonly Habitat Forest = new("forest","Bosque");
        public static readonly Habitat Grassland = new("grassland","Pradera");
        public static readonly Habitat Mountain = new("mountain","Montaña");
        public static readonly Habitat Unknown = new("rare","Desconocido");
        public static readonly Habitat Sea = new("sea","Mar");
        public static readonly Habitat Urban = new("urban","Ciudad");
        public static readonly Habitat WaterEdge = new("waters-edge", "Rio");
        public static readonly Habitat RoughTerrain = new("rough-terrain", "Campo");


        public static IEnumerable<Habitat> GetValues() {
            yield return Cave;
            yield return Forest;
            yield return Grassland;
            yield return Mountain;
            yield return Unknown;
            yield return Sea;
            yield return Urban;
            yield return WaterEdge;
            yield return RoughTerrain;
        }

        public static Habitat Get(string code) {
            if(string.IsNullOrEmpty(code))
                return Unknown;
            return GetValues().FirstOrDefault(x => x.Code == code) ?? Grassland;
        }
    }


    public sealed class Growth 
    {
        [JsonIgnore]
        public  string Name { get; private set; }
        public string Code {get; set;}
        Growth(string code, string name) {
            this.Name = name;
            this.Code = code;
        }

        public static readonly Growth Slow = new("slow","Lento");
        public static readonly Growth Medium = new("medium","Medio");
        public static readonly Growth Fast = new("fast", "Rápido");
        public static readonly Growth Parabolic = new("medium-slow","Parabólico");
        public static readonly Growth Erratic = new("slow-then-very-fast","Errático");
        public static readonly Growth Fluctuating = new("fast-then-very-slow","Fluctuante");

        public static IEnumerable<Growth> GetValues() {
            yield return Slow;
            yield return Medium;
            yield return Fast;
            yield return Parabolic;
            yield return Erratic;
            yield return Fluctuating;
        }

        public static Growth Get(string code) {
            if(string.IsNullOrEmpty(code))
                return Medium;
            return GetValues().FirstOrDefault(x => x.Code == code) ?? Medium;
        }
    }
}