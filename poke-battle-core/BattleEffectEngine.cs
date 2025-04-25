using poke.battle.Models.helpers;
using System.Diagnostics.Metrics;

namespace poke.battle.core
{
    public static class BattleEffectEngine 
    {
        private static readonly Random rng = new();

        public static void ApplyMoveEffect(PBattleMove move, PBattler attacker, PBattler defender, BattleContext ctx, ActionResult result) 
        {
            if(move.Move.Name.Equals("metronome"))
            {
                ApplyMetronome(attacker, defender, ctx, result);
                return;
            }
            switch ((int)move.Move.MoveEffect)
            {
                case var code when code == MoveEffect.None.Code:
                    break;

                case var code when code == MoveEffect.Flinch_30.Code:
                    if (rng.NextDouble() <= .3) {
                        defender.Flinched = true;
                        result.Events.Add(new() {
                            Type = "flinch",
                            Message = $"{defender.Nickname} retrocedió."
                        });
                    }
                    break;

                case var code when code == MoveEffect.Burn_10.Code:
                    TryApplyStatus(defender, PStatus.Burn, 0.1, result);
                    break;

                case var code when code == MoveEffect.Poison_10.Code:
                    TryApplyStatus(defender, PStatus.Paralyze, 0.1, result);
                    break;

                case var code when code == MoveEffect.Recoil.Code:
                    int recoil = Math.Max(1, result.LastDamage / 3);
                    attacker.CurrentHp -= recoil;
                    result.Events.Add(new() {
                        Type = "recoil",
                        Message = $"{attacker.Nickname} sufrió daño por el retorceso"
                    });
                    break;

                case var code when code == MoveEffect.Drain.Code:
                    int heal = result.LastDamage / 2;
                    attacker.CurrentHp = Math.Min(attacker.GetStat(Stat.Hp).RawValue, attacker.CurrentHp + heal);

                    result.Events.Add(new() {
                        Type = "heal",
                        Message = $"{attacker.Nickname} recuperó salud"
                    });
                    break;

                case var code when code == MoveEffect.Lower_Attack.Code:
                    defender.Boost(Stat.Attk, -1, result);
                    break;

                case var code when code == MoveEffect.Lower_Defense.Code:
                    defender.Boost(Stat.Def, -1, result);
                    break;

                case var code when code == MoveEffect.Lower_Speed.Code:
                    defender.Boost(Stat.Spd, -1, result);
                    break;

                case var code when code == MoveEffect.Raise_Attack.Code:
                    attacker.Boost(Stat.Attk, 1, result);
                    break;

                case var code when code == MoveEffect.Raise_Defense.Code:
                    attacker.Boost(Stat.Def, -1, result);
                    break;

                case var code when code == MoveEffect.Raise_Speed.Code:
                    attacker.Boost(Stat.Spd, -1, result);
                    break;

                case var code when code == MoveEffect.Raise_Evasion.Code:
                    attacker.BoostEvassion(1, result);
                    break;

                case var code when code == MoveEffect.Lower_Accuracy.Code:
                    defender.BoostAccuracy(-1, result);
                    break;

                case var code when code == MoveEffect.LeechSeed.Code:
                    defender.ApplyLeechSeed(attacker, result);
                    break;

                case var code when code == MoveEffect.Protect.Code:

                    ctx.GetSide(attacker).ApplyEffect("protect", 1);
                    attacker.HasReflect = true;
                    result.Events.Add(new()
                    {
                        Type = "protect",
                        Message = $"{attacker.Nickname} se protegió"
                    });

                    break;

                case var code when code == MoveEffect.Reflect.Code:
                    if (!attacker.HasReflect)
                    {
                        ctx.GetSide(attacker).ApplyEffect("reflect", 5);
                        attacker.HasReflect = true;
                        result.Events.Add(new()
                        {
                            Type = "reflect",
                            Message = $"{attacker.Nickname} se protegió."
                        });
                    }
                    break;

                case var code when code == MoveEffect.LightScreen.Code:
                    if (!attacker.HasLightScreen)
                    {
                        ctx.GetSide(attacker).ApplyEffect("light-screen", 5);
                        attacker.HasLightScreen = true;
                        result.Events.Add(new()
                        {
                            Type = "light-screen",
                            Message = $"{attacker.Nickname} se protegió con un muro de luz."
                        });
                    }
                    break;

                case var code when code == MoveEffect.Trap_Partial.Code:
                    if (!defender.IsTrapped) {
                        int duration = rng.Next(2, 6);
                        defender.ApplyTrapping(attacker, duration, result);
                    }
                    break;


                case var code when code == MoveEffect.Recover.Code:
                    int maxHp = attacker.GetStat(Stat.Hp).RawValue;
                    int healRecover = maxHp / 2;
                    attacker.Heal(healRecover);

                    result.Events.Add(new()
                    {
                        Type = "recover",
                        Message = $"{attacker.Nickname} recuperó salud."
                    });

                    break;


                case var code when code == MoveEffect.Counter.Code:
                    if (attacker.LastPhysicalDamage > 0 && attacker.LastAttacker is not null && !attacker.LastAttacker.IsFainted && attacker.LastAttacker.IsActive)
                    {
                        int counter = attacker.LastPhysicalDamage * 2;
                        attacker.LastAttacker.TakeDamage(counter);

                        result.Events.Add(new() { Type = "damage", Message = $"{attacker.Nickname} contraatacó con fuerza.", PayLoad = new { Damage = counter, Move = "counter" } });

                        defender.LastPhysicalDamage = 0;
                        defender.LastAttacker = null;
                    } else
                    {
                        result.Events.Add(new() { Type = "fail", Message = $"{attacker.Nickname} falló su contraataque." });

                    }
                    break;

                case var code when code == MoveEffect.SelfDestruct.Code:
                    attacker.TakeDamage(attacker.CurrentHp);
                    result.Events.Add(new()
                    {
                        Type = "self-ko",
                        Message = $"{attacker.Nickname} se ha autodestruido"
                    });
                    break;

                case var code when code == MoveEffect.Transform.Code:
                    if (attacker.HasTransformed)
                    {
                        result.Events.Add(new() { Type = "fail", Message = $"{attacker.Nickname} ya está transformado." });
                        break;
                    }
                    attacker.TransformTo(defender, result);
                    break;

                case var code when code == MoveEffect.Bide.Code:
                    attacker.StartBide();
                    result.Events.Add(new() { Type = "bide-start", Message = $"{attacker.Nickname} está acumulando energía." });
                    break;

                case var code when code == MoveEffect.Disable.Code:
                    var available = defender.Moves.Where(m => m.CurrentPP > 0 && m.Move.Power > 0 && !m.IsDisabled).ToList();
                    if (!available.Any())
                        {
                            result.Events.Add(new() { Type = "fail", Message = $"{attacker.Nickname} intentó anular un movimiento pero no surtió efecto." });
                        }
                    var selected = available[rng.Next(available.Count)];
                    selected.DisableMove(rng.Next(4, 8));
                        result.Events.Add(new() { Type = "disable", Message = $"{selected.Move.DisplayName} de {defender.Nickname} ha sido anulado." });
                    break;

                case var code when code == MoveEffect.Mimic.Code:
                    var targetMoves = defender.Moves.Where(m => m.Move.Name != move.Move.Name && !attacker.Moves.Any(x => x.Move.Name == m.Move.Name)).ToList();
                    if(!targetMoves.Any())
                    {
                        result.Events.Add(new()
                        {
                            Type = "copy-fail",
                            Message = $"{attacker.Nickname} no pudo copiar ningún movimiento."
                        });
                        break;
                    }

                    var copied = targetMoves[rng.Next(targetMoves.ToList().Count)];

                    var moves = attacker.Moves.ToList();
                    moves.Remove(move);
                    moves.Add(copied);
                    attacker.SetMoves(moves.ToArray());
                    result.Events.Add(new()
                    {
                        Type = "copy",
                        Message = $"{attacker.Nickname} copió {copied.Move.DisplayName}."
                    });
                break;

                default:
                    result.Events.Add(new(){
                            Type = "info",
                            Message = $"[DEBUG] Efecto desconocido 0x{move.Move.MoveEffect:x3}"
                        });
                break;
            }
        }

        private static void ApplyMetronome(PBattler attacker, PBattler defender, BattleContext ctx, ActionResult result)
        {
            var moveRandom = CoreSettings.GetRandomMove(["metronome", "struggle", "mimic"]);
            if(moveRandom == null)
            {
                result.Events.Add(new() { Type = "fail", Message = $"{attacker.Nickname} intentó usar Metrónomo, pero no pasó nada" });
                return; 
            }
            var chosen = new PBattleMove(moveRandom);
            var tmpAction = new MoveAction() { Actor = attacker, Move = chosen, Context = ctx};
            result.Events.AddRange(tmpAction.Execute(defender).Events);
        }   

        private static void TryApplyStatus(PBattler target, PStatus status, double chance, ActionResult result) {
            if(target.HasStatus(PStatus.None) && rng.NextDouble() < chance)
                target.ApplyStatus(status, result);
        }

    }

}