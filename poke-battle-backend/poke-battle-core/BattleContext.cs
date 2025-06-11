using poke_battle_core;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;

namespace poke.battle.core
{
    public class BattleContext 
    {
        public int Turn { get; set; } = 1;
        public BattleSide Playerside { get; } = new();
        public BattleSide EnemySide {  get; } = new();

        public BattleSide GetSide(PBattler battler) => Player.Contains(battler) ? Playerside : EnemySide;

        public List<PBattler> Player { get; } = new();
        public List<PBattler> Enemy { get; } = new();
        public BattleType Type { get; set; } = BattleType.Single;
        public Random RNG { get; } = new Random();
        public BattleContext(List<PBattler> player, List<PBattler> rival, BattleType type = BattleType.Single) {
            this.Player = player;
            this.Enemy = rival;
            this.Type = type;
        }
        public IEnumerable<PBattler> GetOpponents(PBattler actor)
        {
            if(Player.Contains(actor)) return Enemy.Where(b => b.IsActive &&  b.CurrentHp > 0 && !b.IsEgg);
            if(Enemy.Contains(actor)) return Player.Where(b => b.IsActive && b.CurrentHp > 0 && !b.IsEgg);
            
            throw new InvalidOperationException("El Pokémon no está en ningún equipo.");
        }
        


        public IEnumerable<PBattler> GetAll() => Player.Concat(Enemy);
        public IEnumerable<PBattler> GetActiveAll() => GetAll().Where(p => p.IsActive && p.CurrentHp > 0 && !p.IsEgg);
        public IEnumerable<PBattler> GetTeamOf(PBattler p) {
            if(Player.Contains(p)) return  Player;
            if(Enemy.Contains(p)) return Enemy;

            throw new InvalidOperationException("No pertenece a ningun equipo");

        }

        public PBattler? GetBattlerByTeamAndSlot(int team, int slot) {
            return GetAll().FirstOrDefault(b => b.TeamId == team && b.Slot == slot);
        }

        public void SwitchActive(PBattler from, PBattler to) {
            if(!GetTeamOf(from).Contains(to))
                throw new InvalidOperationException("El Pokémon de destino no pertenece al mismo equipo.");
            from.ResetBattleStats();
            from.IsActive = false;

            to.IsActive = true;
            to.WasInBattle = true;

        }

        public PBattler? GetNext(PBattler fainted) {
            var team = GetTeamOf(fainted);
            return team.Where(p => !p.IsFainted && !p.IsEgg && !p.IsActive).OrderBy(p => p.Slot).FirstOrDefault();
        }
    }

    public enum BattleType { Single, Double }

}