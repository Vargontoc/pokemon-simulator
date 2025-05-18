using poke.battle.Models;
using poke.battle.Models.Impl;

namespace poke.battle.core
{
    public class PBattler 
    {
        public SpecieModel Specie { get; private set; } 
        /// <summary>
        /// Obtiene o establece el nivel del Battler
        /// </summary>
        public int Level {get; private set; } 
        /// <summary>
        /// Obtiene el nombre del Battler
        /// </summary>
        public string Nickname {get; private set; } = string.Empty;
        /// <summary>
        /// Obtiene si es un huevo
        /// </summary>
        public bool IsEgg { get; private set; } = false;
        /// <summary>
        /// Obtiene la naturaleza del Battler
        /// </summary>
        public Nature Nature { get; private set; }
        /// <summary>
        /// Obtiene los stats del Battler
        /// </summary>
        public StatEntry[] Stats { get; private set; } = [];
        private Dictionary<string, int> Mods { get; set; } = new() {
            [CoreSettings.MOD_EV] = 0,
            [CoreSettings.MOD_ACC] = 0
        };
        public PBattleMove[] Moves { get; private set;} = [];
        public TypeModel[] Types { get; private set; } = [];
       
        public PStatus Status { get; private set; } = PStatus.None;
        private int SleepTurns {get; set; } = 0;
        public bool IsConfused => ConfusionTurns > 0;
        private int ConfusionTurns { get; set; } = 0;
        public bool IsTrapped => TrapCounter > 0;
        public int TrapCounter { get; private set; }
        public PBattler? TrappedBy {get; private set; } = null;
        public int MaxHP { get { return GetStat(Stat.Hp).RawValue; } }
        public int CurrentHp { get; set; }
        public bool IsFainted { get { return CurrentHp <= 0; }}
        public int Critical {get; set; } = 0;
        public bool IsActive {get; set; } = false;
        public bool HasActed { get; set;} = false;
        public bool Flinched {get; set; } = false;
        public bool Protected { get; set; } = false; 
        public PBattler? SeedBy { get; private set; } = null;
        public bool IsSeeded => SeedBySlot.HasValue && SeedByTeam.HasValue;
        public int? SeedBySlot { get; set; } = null;
        public int? SeedByTeam {get; set; } = null;
        public int TeamId {get; set; }
        public int Slot { get; set;}
        public bool HasReflect { get; set; }
        public bool HasLightScreen { get; set; }
        public bool HasTransformed { get; set; } = false;
        public MoveModel? LastMoveReceived { get; set; }
        public int LastPhysicalDamage { get; set; } = 0;
        public PBattler? LastAttacker { get; set; }
        public bool IsBiding { get { return BideTurns > 0; } } 
        public int BideTurns { get; set; } = 0;
        public int BideDamage { get; set; } = 0;
        public PBattler? BideLastAttacker { get; set; } = null;
        public bool IsChargingMove { get; set; } = false;
        public bool IsRecharging { get; set; } = false;
        public PBattler(SpecieModel specie, int level, bool isEgg = false)
        {
            this.Specie = specie;
            this.Level = level;    
            this.Nickname = specie.DisplayName ?? specie.Name;
            this.IsEgg = isEgg;
            this.Types = specie.Types.Select(t => new TypeModel() { Name = t}).ToArray();
            Nature = Nature.Random();

            Stats = Stat.GetValues().Select((stat, i) => new StatEntry(stat, specie.StatsBase[i])).ToArray();
            CalculateStats();
            CurrentHp = GetStat(Stat.Hp).RawValue;
            
        }

        public void TakeDamage(int damage, PBattler? attacker = null) {
            CurrentHp = Math.Max(0, CurrentHp - damage);
            if (IsBiding)
            {
                BideDamage += damage;
                BideLastAttacker = attacker;
            }
        }

        public void ApplyStatus(PStatus status, ActionResult result)
        {
            if(Status != PStatus.None) return;
            
            Status = status;
            if(Status == PStatus.Sleep)
                SleepTurns = new Random().Next(2, 5);

            result.Events.Add(new() {
                Type = "status",
                Message = $"{Nickname} está ahora {Status.Name.ToLower()}"
            });
            
        }

        public void ApplyTrapping(PBattler source, int turns, ActionResult result) 
        {
            TrapCounter = turns;
            TrappedBy = source;
            result.Events.Add(new(){
                Type = "trap-start",
                Message = $"{Nickname} quedó atrapado por {source.Nickname}"
            });

        }

        public void ApplyLeechSeed(PBattler source, ActionResult result) {
            if(IsSeeded || Specie.Types.Contains("grass", StringComparer.OrdinalIgnoreCase)) return;
            else {
                SeedBySlot = source.Slot;
                SeedByTeam = source.TeamId;

                result.Events.Add(new () {
                    Type = "leech-seed",
                    Message = $"{Nickname} fue afectado por drenadoras"
                });
            }

        }

        public void ClearLeachSeed() 
        {
            SeedBy = null;
        }

        public void TickTrapping(ActionResult result) {
            if(TrapCounter > 0) {
                TrapCounter--;
                if(TrapCounter == 0) {
                    result.Events.Add(new() {
                        Type = "trap-end",
                        Message = $"{Nickname} se liberó."
                    });
                    TrappedBy = null;
                }
            }
        }

        public void ResolveConfusion(ActionResult result) 
        {
            if(ConfusionTurns <= 0) return;
            ConfusionTurns--;

            if(ConfusionTurns == 0)
            {
                result.Events.Add(new() {
                    Type = "status-end",
                    Message = $"{Nickname} ya no está confuso"
                });

                return;
            }

            if(new Random().NextDouble() <= 1.0 / 3.0) 
            {
                
                int atk = GetStat(Stat.Attk).RawValue;
                int def = GetStat(Stat.Def).RawValue;

                int damage = DamageCalculator.ConfusionDamage(Level, atk, def);

                CurrentHp = Math.Max(0, CurrentHp - damage);
                result.Events.Add(new() {
                    Type = "confused-hit",
                    Message = $"{Nickname} está confundido y se golpeó así mismo"
                });

                if(CurrentHp == 0) {
                    result.Events.Add(new () {
                        Type = "faint",
                        Message = $"{Nickname} se ha debilitado."
                    });
                }

                throw new BattlerInterruptedException("confused-hit");
            }
        }

        public void ApplyConfusion(ActionResult result) 
        {
            if(!IsConfused) 
            {
                ConfusionTurns = new Random().Next(2, 6);
                result.Events.Add(new() {
                    Type = "status",
                    Message = $"{Nickname} está confuso"
                });
            }
        }

        public void CanAct(ActionResult result) 
        {
            if(IsFainted)
                throw new BattlerInterruptedException($"{Nickname} está debilitado");

            if (Flinched)
                throw new BattlerInterruptedException($"¡{Nickname} retrocedió!");

            if (IsRecharging)
                throw new BattlerInterruptedException($"{Nickname} está recargando.");

            if (IsBiding)
                throw new BattlerInterruptedException($"{Nickname} está acumulando energía.");

            if (IsChargingMove)
                throw new BattlerInterruptedException($"{Nickname} está cargando un movimiento.");

           

            if(IsConfused)
                ResolveConfusion(result);
        }

        public void CureStatus() 
        {   
            Status = PStatus.None;
        }

        public void Heal(int heal) 
        {
            CurrentHp = Math.Min(CurrentHp + heal, GetStat(Stat.Hp).RawValue);
        }
        public StatEntry GetStat(Stat stat)
        {
            return Stats.First(s => s.Stat == stat);
        }

        public void Boost(Stat stat, int value) 
        {
            GetStat(stat).ChangeModifier(value);
        }

        public void BoostCritic(int delta, ActionResult result) {
            int prev = Critical;
            Critical = Math.Clamp(Critical + delta, 0, 4);
        } 
        public void Boost(Stat stat, int delta, ActionResult result) 
        {
            var s = GetStat(stat);
            int prev = s.Modifier;

            Boost(stat, delta);
            bool changed = s.Modifier != prev;
            bool strong = Math.Abs(delta) > 1;

            string intensity = strong ? " mucho " : "";
            if(delta > 0) 
            {
                result.Events.Add(new() {
                    Type = "stat-up",
                    Message = !changed 
                    ? $"{Nickname} no pudo aumentar su {stat.Name}." 
                    :  $"{Nickname} aumentó{intensity} su {stat.Name}."
                });
            }else 
            {
                result.Events.Add(new() {
                    Type = "stat-down",
                    Message = !changed 
                    ? $"{stat.Name} de {Nickname} no bajó" 
                    :  $"{stat.Name} de{ Nickname} bajó{intensity}"
                });
            }

        }
        public void BoostEvassion(int delta, ActionResult result) 
        {
            BootsMods(delta, CoreSettings.MOD_EV, result);
        }
        public void BoostAccuracy(int delta, ActionResult result) 
        {
            BootsMods(delta, CoreSettings.MOD_ACC, result);        
        }

        private void BootsMods(int delta, string mod, ActionResult result) 
        {
            int prev = Mods[mod];
            Mods[mod] = Math.Clamp(Mods[mod] + delta, -6, 6);

            bool changed = Mods[CoreSettings.MOD_EV] != prev;
            bool strong = Math.Abs(delta) > 1;

            string intensity = strong ? " mucho" :"";
            string modText = mod == CoreSettings.MOD_EV ? "evasión":"precisión";
            string modType =  mod == CoreSettings.MOD_EV ? "evassion":"accuracy";
            if(delta > 0) 
            {
                result.Events.Add(new() {
                    Type = $"{modType}-up",
                    Message = !changed 
                    ? $"{Nickname} no pudo aumentar su {modText}." 
                    :  $"La {modText} de {Nickname} subió{intensity}."
                });
            }else 
            {
                result.Events.Add(new() {
                    Type = $"{modType}-down",
                    Message = !changed 
                    ? $"La {modText} de {Nickname} no bajó." 
                    :  $"La {modText} de{ Nickname} bajó{intensity}"
                });
            }
        }
        public void ResetBattleStats() 
        {
            Stats.ToList().ForEach(s => s.Reset());
            Mods[CoreSettings.MOD_ACC] = 0;
            Mods[CoreSettings.MOD_EV] = 0;

            ConfusionTurns = 0;
            Flinched = false;
            SeedBy = null;
            TrapCounter = 0;
            


        }

        public double GetAccuracy() {
            return Calculator.GetModMultiplier(Mods[CoreSettings.MOD_ACC]);
        }




        public double GetEvassion() {
            return Calculator.GetModMultiplier(Mods[CoreSettings.MOD_EV]);
        }
        

        public void CalculateStats() 
        {
            Stats.ToList().ForEach(s => Calculator.CalculateStat(s, Level, Nature));
        }

        private void ResetTurnState() {
            HasActed = false;
            Flinched = false;
            Protected = false;

        
        }

        public void OnTurnEnd(ActionResult result, BattleContext context)
        {
            if (IsFainted) return;

            for (int i = 0; i < Moves.Length; i++)
                Moves[i].TickDisable();

            Status.OnTurnEnd(this, result);
            if (CurrentHp == 0)
            {
                result.Events.Add(new()
                {
                    Type = "faint",
                    Message = $"{Nickname} se debilitó."
                });
            }

            ResetTurnState();

            if (IsSeeded && SeedBySlot.HasValue && SeedByTeam.HasValue && !IsFainted)
            {
                int damage = Math.Max(1, GetStat(Stat.Hp).RawValue / 8);
                CurrentHp = Math.Max(0, CurrentHp - damage);

                result.Events.Add(new()
                {
                    Type = "leech-damage",
                    Message = $"{Nickname} sufre el efecto de Drenadoras.",
                    PayLoad = new { Damage = damage }
                });

                var healer = context.GetBattlerByTeamAndSlot(SeedByTeam.Value, SeedBySlot.Value);

                if (healer != null && !healer.IsFainted)
                {
                    healer.CurrentHp = Math.Min(healer.GetStat(Stat.Hp).RawValue, healer.CurrentHp + damage);
                    result.Events.Add(new()
                    {
                        Type = "leech-heal",
                        Message = $"{healer.Nickname} recuperó salud con Drenadoras"
                    });
                }
            }

            LastAttacker = null;
            LastPhysicalDamage = 0;

            if (IsBiding)
            {
                BideTurns--;
                if (BideTurns == 0)
                {
                    if (BideLastAttacker != null && BideLastAttacker.CurrentHp > 0 && BideLastAttacker.IsActive)
                    {
                        int dmg = BideDamage * 2;
                        BideLastAttacker.CurrentHp = Math.Max(0, BideLastAttacker.CurrentHp - dmg);
                        result.Events.Add(new() { Type = "bide-release", Message = $"{Nickname} liberó energía y golpeó a  {BideLastAttacker.Nickname}" });

                        if (BideLastAttacker.IsFainted)
                        {
                            result.Events.Add(new() { Type = "faint", Message = $"{BideLastAttacker.Nickname} se ha debilitado" });
                        }
                    }
                    else
                    {
                        result.Events.Add(new() { Type = "fail", Message = $"{Nickname} liberó energía pero no habia objetivo" });
                    }

                    BideDamage = 0;
                    BideLastAttacker = null;

                }
                else
                {
                    result.Events.Add(new() { Type = "bide-continue", Message = $"{Nickname} continúa acumulando energía" });
                }
            }
        }
        
        public void ChangeName(string nickname)
        {
            Nickname = nickname;
        }

        public void SetMoves(PBattleMove[] moves) {
            Moves = moves;
        }

        public bool HasStatus(PStatus status) => Status == status;


        internal void OnTurnStart(ActionResult result)
        {
            if(IsFainted) return;
            if(IsTrapped) 
            {
                int trap = GetStat(Stat.Hp).RawValue / 16;
                CurrentHp = Math.Max(0, CurrentHp - trap);

                result.Events.Add(new(){
                    Type = "trap-damage",
                    Message = $"{Nickname} sufre daño por el atrapamiento",
                    PayLoad = new { Damage = trap }
                });
            }


            switch (Status.Code)
            {
                case var code when code == PStatus.Sleep.Code:
                    if (SleepTurns > 0)
                    {
                        SleepTurns--;
                        result.Events.Add(new() { Type = "status", Message = $"{Nickname}  aún está dormido y no puede actuar." });
                        throw new BattlerInterruptedException($"{Nickname} está dormido");
                    }
                    else
                    {
                        CureStatus();
                        result.Events.Add(new() { Type = "status", Message = $"{Nickname}  se depertó." });
                        Status = PStatus.None;
                    }
                    break;

                case var code when code == PStatus.Paralyze.Code:
                    if (new Random().NextDouble() <= .25)
                    {
                        SleepTurns--;
                        result.Events.Add(new() { Type = "status", Message = $"{Nickname} está paralizado. ¡No puede moverse!" });
                        throw new BattlerInterruptedException($"{Nickname} está paralizado");
                    }
                    break;

                case var code when code == PStatus.Freeze.Code:
                    if (new Random().NextDouble() <= .2)
                    {
                        SleepTurns--;
                        result.Events.Add(new() { Type = "status", Message = $"{Nickname}  aún está congelado." });
                        throw new BattlerInterruptedException($"{Nickname} está congelado");
                    }
                    else
                    {
                        CureStatus();
                        result.Events.Add(new() { Type = "status", Message = $"{Nickname}  se descongeló." });
                        Status = PStatus.None;

                    }
                    break;
                default: break;

            }
        }

        public void TransformTo(PBattler defender, ActionResult result)
        {
            if (HasTransformed) return;

            HasTransformed = true;
            this.Types = defender.Types;
            this.Specie = defender.Specie;

            for(int i = 0; i < Stats.Length; i++)
            {
                Stats[i].RawValue = defender.Stats[i].RawValue;
                Stats[i].BaseValue = defender.Stats[i].BaseValue;
            }

            this.Moves = defender.Moves.Select(m => new PBattleMove(m.Move) { CurrentPP = m.CurrentPP}).ToArray();
            CalculateStats();

            result.Events.Add(new() { Type = "transform", Message = $"{Nickname} se transformó en {defender.Nickname}" });

        }

        public void StartBide() 
        {
            

            BideTurns = Random.Shared.Next(2, 4);
            BideDamage = 0;
            BideLastAttacker = null;
        }
    }    
}