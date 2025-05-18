namespace poke.battle.core
{
    public class PStatus 
    {
        public string Code {get; }
        public string Name { get; }

        protected PStatus(string code, string name) {
            this.Code = code;
            this.Name = name;
        }

        public virtual void OnTurnEnd(PBattler battler, ActionResult result) {

            switch(Code) {
                case "brn":
                    ApplyResidualDamage(1.0 / 16.0, "a causa de la quemadura", battler, result);
                break;
                case "psn":
                     ApplyResidualDamage(1.0 / 8.0, "por el veneno", battler, result);
                break;
            }
        }

        private static void ApplyResidualDamage(double fraction, string partialMsg, PBattler battler, ActionResult result)
        {
            int damage = (int)(battler.GetStat(Stat.Hp).RawValue * fraction);
            damage = Math.Max(damage,1);
            battler.CurrentHp = Math.Max(0, battler.CurrentHp - damage);
            result.Events.Add(new() {
                Type = "status-damage",
                Message = $"{battler.Nickname} sufrió daño {partialMsg}"
            });
        }

        public static readonly PStatus None = new("none", "");
        public static readonly PStatus Burn = new("brn", "Quemado");
        public static readonly PStatus Poison = new("psn", "Envenenado");
        public static readonly PStatus Sleep = new("slp", "Dormido");
        public static readonly PStatus Paralyze = new("plz", "Paralizado");
        public static readonly PStatus Freeze = new("fzn", "Congelado");
         public static readonly PStatus Bleed = new("bld", "Sangrado");

        public static IEnumerable<PStatus> GetValues() {
            yield return None;
            yield return Burn;
            yield return Poison;
            yield return Sleep;
            yield return Paralyze;
            yield return Freeze;

        }

        public static PStatus Get(string code) => GetValues().FirstOrDefault(x => x.Code == code) ?? None;
    }

}