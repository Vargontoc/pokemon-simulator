namespace poke.battle.core {
    public class BattlerInterruptedException : Exception {
        public BattlerInterruptedException(string reason) : base(reason) {}
    }
}