namespace poke.battle.Models.helpers
{
    public sealed class MoveEffect {
        public int Code { get; }
        public string Name { get; } = string.Empty;
        public string Description { get; } = string.Empty;

        private MoveEffect(int code, string name, string description) {
            this.Code = code;
            this.Name = name;
            this.Description = description;
        }

        // Efectos sin daño directo
        public static readonly MoveEffect None = new(0x000, "Sin efecto", "El movimiento no tiene efecto adicional.");

        // Efectos de estado
        public static readonly MoveEffect Flinch_30 = new(0x001, "Retroceso", "30% de hacer retroceder al rival.");
        public static readonly MoveEffect Burn_10 = new(0x002, "Quemadura", "10% de probabilidad de quemar al rival.");
        public static readonly MoveEffect Paralysis_10 = new(0x003, "Parálisis", "10% de probabilidad de paralizar al rival.");
        public static readonly MoveEffect Sleep_100 = new(0x004, "Dormir", "Provoca que el objetivo se duerma.");
        public static readonly MoveEffect Freeze_10 = new(0x005, "Congelación", "10% de probabilidad de congelar al rival.");
        public static readonly MoveEffect Poison_10 = new(0x006, "Envenenamiento", "10% de probabilidad de envenenar al rival.");
        public static readonly MoveEffect Confusion_10 = new(0x007, "Confusión", "10% de probabilidad de confundir al rival.");

        // Bajada de stats (1 nivel)
        public static readonly MoveEffect Lower_Attack = new(0x008, "Reducir ataque", "Reduce el ataque del rival.");
        public static readonly MoveEffect Lower_Defense = new(0x009, "Reducir defensa", "Reduce la defensa del rival.");
        public static readonly MoveEffect Lower_Speed = new(0x00A, "Reducir velocidad", "Reduce la velocidad del rival.");
        public static readonly MoveEffect Lower_Accuracy = new(0x00B, "Reducir precisión", "Reduce la precisión del rival.");
        public static readonly MoveEffect Lower_SpecialAttack = new(0x00C, "Reducir ataque especial", "Reduce el ataque especial.");
        public static readonly MoveEffect Lower_SpecialDefense = new(0x00D, "Reducir defensa especial", "Reduce la defensa especial.");

        // Aumento de stats (1 nivel)
        public static readonly MoveEffect Raise_Attack = new(0x00E, "Aumentar ataque", "Aumenta el ataque del usuario.");
        public static readonly MoveEffect Raise_Defense = new(0x00F, "Aumentar defensa", "Aumenta la defensa del usuario.");
        public static readonly MoveEffect Raise_Speed = new(0x010, "Aumentar velocidad", "Aumenta la velocidad del usuario.");
        public static readonly MoveEffect Raise_Evasion = new(0x011, "Aumentar evasión", "Aumenta la evasión del usuario.");
        public static readonly MoveEffect Raise_Accuracy = new(0x012, "Aumentar precisión", "Aumenta la precisión del usuario.");
        public static readonly MoveEffect Raise_SpecialAttack = new(0x013, "Aumentar ataque especial", "Aumenta el ataque especial.");
        public static readonly MoveEffect Raise_SpecialDefense = new(0x014, "Aumentar defensa especial", "Aumenta la defensa especial.");

        // Otros
        public static readonly MoveEffect Recoil = new(0x015, "Retroceso", "El usuario recibe daño por retroceso.");
        public static readonly MoveEffect Protect = new(0x016, "Protección", "Previene el daño este turno.");
        public static readonly MoveEffect Drain = new(0x017, "Drenaje", "Recupera salud en función del daño infligido.");
        public static readonly MoveEffect MultiHit = new(0x018, "Golpe múltiple", "Golpea al rival entre 2 y 5 veces.");
        public static readonly MoveEffect MultiHit_2 = new(0x019, "Doble golpe", "Golpea al rival 2 veces.");
        public static readonly MoveEffect MultiHit_3 = new(0x01A, "Triple golpe", "Golpea al rival 3 veces.");
        public static readonly MoveEffect Trap_Partial = new(0x01B, "Atrapar", "Atrapa al rival durante 2 a 5 turnos e impide que actúe.");
        public static readonly MoveEffect HighCrit = new(0x01C, "Crítico alto", "Aumenta la probabilidad de golpe crítico.");

        // Transformaciones o efectos únicos
        public static readonly MoveEffect Transform = new(0x01D, "Transformar", "Copia al objetivo.");
        public static readonly MoveEffect Disable = new(0x01E, "Inhabilitar", "Inhabilita un movimiento del rival temporalmente.");
        public static readonly MoveEffect FocusEnergy = new(0x01F, "Concentración", "Aumenta la probabilidad de golpe crítico.");
        public static readonly MoveEffect LeechSeed = new(0x020, "Drenaje", "Recupera vida por turno abroviendo vida del rival.");

        public static readonly MoveEffect LightScreen = new(0x021, "Pantalla luz", "Protege al usuario de movientos de categoria especiales");
        public static readonly MoveEffect Reflect = new(0x022, "Reflejo", "Protege al usuario de movientos de categoria fisicos");

        public static readonly MoveEffect Fixed_20 = new(0x023, "Daño fijo 20", "Hace 20 puntos de daño.");
        public static readonly MoveEffect Fixed_40 = new(0x024, "Daño fijo 40", "Hace 40 puntos de daño.");
        public static readonly MoveEffect LevelDamage = new(0x025, "Daño por nivel", "Hace daño igual al nivel del usuario.");
        public static readonly MoveEffect RandomLevel = new(0x026, "Daño aleatorio por nivel", "Hace entre 0.5x y 1.5x del nivel de daño.");
        public static readonly MoveEffect Recover = new(0x027, "Recuperación", "Recupera 50% de PS");
        public static readonly MoveEffect Rest = new(0x028, "Descanso", "Recupera todos los PS y se duerme");
        public static readonly MoveEffect SelfDestruct = new(0x029, "Autodestrucción", "El usuario se debilita tras causar daño.");

        public static readonly MoveEffect Mimic = new(0x02A, "Mimic", "Copia un movimiento aleatorio del oponente.");
        public static readonly MoveEffect Counter = new(0x02B, "Contra-ataque", "Devuelve el doble del daño recbido.");
        public static readonly MoveEffect Bide = new(0x02C, "Bide", "Acumula daño durante 2 o 3 turnos.");


        public static IEnumerable<MoveEffect> GetValues() {
            yield return None;
            yield return Flinch_30;
            yield return Burn_10;
            yield return Paralysis_10;
            yield return Sleep_100;
            yield return Freeze_10;
            yield return Poison_10;
            yield return Confusion_10;
            yield return Lower_Attack;
            yield return Lower_Defense;
            yield return Lower_Speed;
            yield return Lower_Accuracy;
            yield return Lower_SpecialAttack;
            yield return Lower_SpecialDefense;
            yield return Raise_Attack;
            yield return Raise_Defense;
            yield return Raise_Speed;
            yield return Raise_Evasion;
            yield return Raise_Accuracy;
            yield return Raise_SpecialAttack;
            yield return Raise_SpecialDefense;
            yield return Recoil;
            yield return Protect;
            yield return Drain;
            yield return MultiHit;
            yield return MultiHit_2;
            yield return MultiHit_3;
            yield return Trap_Partial;
            yield return HighCrit;
            yield return Transform;
            yield return Disable;
            yield return FocusEnergy;
            yield return LightScreen;
            yield return Reflect;
            yield return Fixed_20;
            yield return Fixed_40;
            yield return RandomLevel;
            yield return LevelDamage;
            yield return Recover;
            yield return Rest;
            yield return SelfDestruct;
            yield return Mimic;
            yield return Counter;
            yield return Bide;

        }

        public static MoveEffect Get(int code)
        {
            return GetValues().FirstOrDefault(x => x.Code == code) ?? None;
        }

        // override object.Equals
        public override bool Equals(object? obj) => obj is MoveEffect other && Code == other.Code;
        
        // override object.GetHashCode
        public override int GetHashCode() => Code.GetHashCode();
        
        public static implicit operator int(MoveEffect effect) => effect.Code;
        public static  bool operator ==(MoveEffect a, MoveEffect b) => a?.Code == b?.Code;
        public static  bool operator !=(MoveEffect a, MoveEffect b) => !(a?.Code == b?.Code);
    }   
}