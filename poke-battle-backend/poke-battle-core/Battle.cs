namespace poke.battle.core
{
    /// <summary>
    /// Reprsenta un combate Pokemon
    /// </summary>
    public class Battle 
    {
        /// <summary>
        /// Informacion del contextol combate
        /// </summary>
        private readonly BattleContext _context;
        public List<PlayerAction> Actions {get; private set; } = new();
        public List<BattleEvent> Events {get; private set;} = new();
        public BattleContext Context => _context;

        public Battle(BattleContext ctx) => _context = ctx;

        public void QueueAction(PlayerAction action) {
            Actions.Add(action);
        }

        public void QueueAttackAction(string attack, PBattler battler) 
        {
            var move = battler.Moves.FirstOrDefault(x => x.Move.Name.ToLower().Equals(attack.ToLower())) ?? throw new ArgumentException($"Move '{attack}' not found for {battler.Name}");
            Actions.Add(new MoveAction 
            { 
                Actor = battler, 
                Move = move, 
                Context = _context 
            });
        }

        public void QueueSwitchAction(string id, PBattler battler)
        {   
            int slot = int.Parse(id.ToString());
            var target = battler.IsPlayer ? _context.Player[slot] : _context.Enemy[slot];
            Actions.Add(new SwtichAction()
            {
                Actor = battler,
                Target = target,
            });
        }

        public void QueueSwitchAction(PBattler battler, PBattler target) 
        {
            Actions.Add(new SwtichAction 
            { 
                Actor = battler, 
                Target = target
            });
        }

        public void Execute() 
        {
            if(Actions.Count == 0) return;

            Events.Clear();
            var bt = new BattleTurn(_context, Actions , _context.Turn++);
            bt.ExecuteTurn();

            Events.AddRange(bt.Events);
            Actions.Clear();
        }
    }
}