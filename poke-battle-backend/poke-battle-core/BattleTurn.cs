namespace poke.battle.core 
{
    public class BattleTurn 
    {
        private readonly BattleContext _context;
        public int Turn { get; private set; }
        List<PlayerAction> Actions {get; set;} = [];
        public List<BattleEvent> Events { get; } = new();



        public BattleTurn( BattleContext context, List<PlayerAction> actions, int turn) {
            
            _context = context;
            actions.ForEach(a => RegisterAction(a));
            this.Turn = turn;
        }
        public void StartTurn()
        {
            foreach(var b in _context.GetActiveAll()){
                b.OnTurnStart(new ActionResult{ Events = Events});
            }
        }

        public void RegisterAction(PlayerAction action) {
            if(!action.Actor.IsFainted && ! action.Actor.HasActed) {
                Actions.Add(action);
                action.Actor.IsActive = true;
                action.Actor.HasActed = true;
            }
        }

        public void ExecuteTurn() 
        {
            StartTurn();

            var ordered = Actions.OrderByDescending(a => a.Priority).ThenByDescending(a => a.Actor.GetStat(Stat.Spd).ModValue);


            foreach(var action in ordered)
            {
                if(action.Actor.IsFainted || action.Actor.Flinched) continue;


                if(action is SwtichAction sa) 
                {
                    ProcessSwitchAction(sa);
                }

                if(action is MoveAction ma) 
                {
                    ProcessMoveAction(ma);
                }
            }

            EndTurn();
        }

        private void Switch(PBattler target) {
            target.ResetBattleStats();
            var next = _context.GetNext(target);
            if(next != null) {
                var autoSwitch = new SwtichAction {
                    Actor = target,
                    Target = next
                };

                var result = autoSwitch.Execute(null!);
                Events.AddRange(result.Events);
                _context.SwitchActive(target, next);
            }else { 
                Events.Add(new(){ Type = "out-of-pokemon", Message = "Equipo derrotado"});
            }
        }

        private void ProcessSwitchAction(SwtichAction action)
        {

            var result = action.Execute(null!);
            Events.AddRange(result.Events);
            _context.SwitchActive(action.Actor, action.Target);
        }

        private void ProcessMoveAction(MoveAction ma)
        {
            var result = new ActionResult();

                ma.Actor.CanAct(result);

                var targets = _context.GetOpponents(ma.Actor);
                var opponent = _context.Type switch
                {
                    BattleType.Single => targets.First(),
                    BattleType.Double => targets.OrderBy(x => _context.RNG.Next()).First(),
                    _ => throw new NotSupportedException("Formato no soportado")
                };

                result = ma.Execute(opponent);
                Events.AddRange(result.Events);
                
                if(!ma.Actor.IsPlayer && ma.Actor.IsFainted)
                    Switch(ma.Actor);

               
        }


        private void EndTurn()
        {
            _context.Playerside.TickEffects();
            _context.EnemySide.TickEffects();
            foreach(var b in _context.GetActiveAll()){
                b.OnTurnEnd(new ActionResult{ Events = Events}, _context);
            }
        }
    }
}