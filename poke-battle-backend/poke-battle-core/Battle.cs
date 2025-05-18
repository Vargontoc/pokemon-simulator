namespace poke.battle.core
{
    public class Battle 
    {
        private readonly BattleContext _context;
        public List<PlayerAction> Actions {get; private set; } = new();
        public List<BattleEvent> Events {get; private set;} = new();
        public int turn = 1;
        public Battle(BattleContext ctx) {
            _context = ctx;
        }

        public void QueueAction(PlayerAction action) {
            Actions.Add(action);
        }

        public void Execute() 
        {
            
            Events.Clear();
            var bt = new BattleTurn(_context, Actions , turn);
            bt.ExecuteTurn();

            Events.AddRange(bt.Events);
            Actions.Clear();
            ++turn;
        }
    }
}