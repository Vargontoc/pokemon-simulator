

using poke.battle.Models.helpers;
using System.ComponentModel;

namespace poke.battle.core {

    public abstract class PlayerAction
    {
        public PBattler Actor { get; set;} = default!;
        public virtual int Priority {get; set; }

        public abstract ActionResult Execute(PBattler opponent);

        protected static void AddEvent(ActionResult result, string type, string msg, object? payload = null) {
            result.Events.Add(new() {
                Type = type,
                Message = msg,
                PayLoad = payload   
            });
        }
    }


    public class ActionResult
    {
        public bool Success {get; set; }
        public List<BattleEvent> Events  { get; set; } = [];
        public int LastDamage { get; set; }
    }

    public class MoveAction : PlayerAction
    {
        public PBattleMove Move {get; set;} = default!;
        public BattleContext Context {get; set;} = default!;
        public override int Priority => Move.Move.Priority;
        public override ActionResult Execute(PBattler opponent)
        {
            var result = new ActionResult();
            if(Move.IsDisabled)
            {
                AddEvent(result, "fail", $"{Move.Move.DisplayName} está deshabilitado.");
                return result;
            }

            if(Move.Move.Name == "mirror-move")
            {
                var copied = Actor.LastMoveReceived;
                if(copied == null)
                {
                    AddEvent(result, "fail", $"{Actor.Nickname} intentó copiar un movimiento");
                    return result;
                }

                AddEvent(result, "mirror-wave", $"{Actor.Nickname} usó Manto Espejo.");
                var mirror = new MoveAction { Actor = Actor,  Context = Context, Move = new PBattleMove(copied) };
                return mirror.Execute(opponent);
            }


            if(Move.CurrentPP == 0 || Move.IsDisabled) {
                AddEvent(result, "fail", $"{Actor.Nickname} no pudo usar {Move.Move.DisplayName}");
                return result;
            }

            bool isMuiltiHit = Move.Move.MoveEffect == MoveEffect.MultiHit ||
            Move.Move.MoveEffect == MoveEffect.MultiHit_2 ||
            Move.Move.MoveEffect == MoveEffect.MultiHit_3;

            if(isMuiltiHit) 
            {
                MultiHit(opponent, result);
                return result;
            }

            // Movimiento normal
            if(!BattleMath.ApplyAccuracy(Actor, opponent, Move.Move.Accuracy))
            {
                AddEvent(result, "miss", $"{Actor.Nickname} falló el golpe.");
                Move.Use();
                return result;
            }

            if(Context.GetSide(opponent).HasEffect("protect"))
            {
                AddEvent(result, "protect-blocked", $"{opponent.Nickname} se protegió del ataque.");
                return result;
            }
   
            Move.Use();
            opponent.LastMoveReceived = Move.Move;
            ApplySingleHit(opponent, result, Move.Move.Name);

 
            if(!opponent.IsFainted)
            {
                BattleEffectEngine.ApplyMoveEffect(Move, Actor, opponent, Context, result);
            }else {
                AddEvent(result, "faint", $"{opponent.Nickname} se ha debilitado.");
            }


            return result;
        }

        private void MultiHit(PBattler target, ActionResult result) 
        {
            int hits = Move.Move.MoveEffect switch {
                var e when e == MoveEffect.MultiHit_2 => 2,
                var e when e == MoveEffect.MultiHit_3 => 3,
                _ => RollHits()
            };

            AddEvent(result, "multihit-start", $"{Actor.Nickname} atacó {hits} veces.");
            Move.Use();
            target.LastMoveReceived = Move.Move;
            for(int i = 0; i < hits; i++) {
                if(target.IsFainted) break;


                if (Context.GetSide(target).HasEffect("protect"))
                {
                    AddEvent(result, "protect-blocked", $"{target.Nickname} se protegió del ataque.");
                    break;
                }

                if (BattleMath.ApplyAccuracy(Actor, target, Move.Move.Accuracy))
                {
                    AddEvent(result, "miss", $"{Actor.Nickname} falló el golpe.");
                    continue;
                }

                ApplySingleHit(target, result, $"{Move.Move.Name}");
            }
        }

        private void ApplySingleHit(PBattler target, ActionResult result, string label) {
            double effectiveness;
            bool isCritic;
            
            int damage = DamageCalculator.Calculate(Actor, target, Move.Move,Context, out effectiveness, out isCritic);
            target.TakeDamage(damage, Actor);

            AddEvent(result, "damage", $"{Actor.Nickname} usó {label}", new { Damage = damage, Move = Move.Move.Name});
            result.LastDamage = damage;

            if (Move.Move.MoveType == Models.MoveType.Physical) { 
                target.LastMoveReceived = Move.Move;
                target.LastPhysicalDamage = damage;
            }
            if(isCritic)
                AddEvent(result, "critical", $"¡Es un golpe crítico!");
            
            if(effectiveness != 1.0)
                AddEvent(result, "effectiveness", TypeEffectivenessResolver.GetText(effectiveness));

            if(target.IsFainted)
                AddEvent(result, "faint", $"{target.Nickname} se ha debilitado.");
            else if(Move.Move.SecondaryEffect != null)
                BattleEffectEngine.ApplyMoveEffect(Move, Actor, target, Context, result);
            

        }
        
       private static int RollHits() {
            var roll = new Random().NextDouble();
            if(roll < .375) return 2;
            if(roll < .75) return 3;
            if(roll < -875) return 4;
            return 5; 
        } 


    }

    public class SwtichAction: PlayerAction {
        public PBattler Target {get; set; } = default!;
        public override int Priority => 6;
        public override ActionResult Execute(PBattler opponent)
        {
            var result = new ActionResult();
            if(Target == Actor) {
                result.Success = false;
                result.Events.Add(new() {
                    Type = "fail",
                    Message = $"{Actor.Nickname} ya está en combate."
                });
                return result;
            }

            if(Target.CurrentHp <= 0) {
                result.Success = false;
                result.Events.Add(new() {
                    Type = "fail",
                    Message = $"{Target.Nickname} no puede entrar porque está debilitado."
                });
                return result;
            }

            
            if(Target.IsEgg) {
                result.Success = false;
                result.Events.Add(new() {
                    Type = "fail",
                    Message = $"Un huevo no puede entrar al combate."
                });
                return result;
            }

            Actor.IsActive = false;
            Target.IsActive = true;

            result.Events.Add(new() {
                    Type = "switch",
                    Message = $"Vuelve {Actor.Nickname}. ¡Adelante {Target.Nickname}!"
                });

            return result;
        }
    }

    public class UseItemAction: PlayerAction {
        public override ActionResult Execute(PBattler opponent)
        {
            throw new NotImplementedException();
        }
    }

    public class RunAction : PlayerAction {
        public override ActionResult Execute(PBattler opponent)
        {
            throw new NotImplementedException();
        }
    }
}