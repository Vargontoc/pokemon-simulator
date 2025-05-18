using System.Runtime.Serialization;
using poke.battle.core;
using poke.battle.services;

namespace poke.battle.factories {
    public class BattlerFactory 
    {
        private readonly IPokemonService pokemonService;
        private readonly IMoveService moveService;

        public BattlerFactory(IPokemonService specieService, IMoveService moveService) {
            this.pokemonService = specieService;
            this.moveService = moveService;
        }

        public PBattler CreateBattler(string name, int level, string? nickname, int? team = null, int? slot = null) 
        {
            var specie = pokemonService.GetByName(name);
            if(specie == null)
                throw new Exception($"Specie '{name}' no found.");
            
           // -- Movimientos --
            var movepool = specie.Movepool
                .Where(m => m.Item1 <= level)
                .Select(m => m.Item2)
                .Distinct();
            var rng = new Random();
            var moveNames =  movepool.OrderBy(_ => rng.Next()).Take(4);
       

            var moves = moveNames
                .Select(n => moveService.GetByName(n))
                .Where(m => m != null)
                .Select(m => new PBattleMove(m!))
                .ToList();

            var battler = new PBattler(specie, level);
            if(!string.IsNullOrEmpty(nickname)) battler.ChangeName(nickname);
            if(team.HasValue) battler.TeamId = team.Value;
            if(slot.HasValue) battler.Slot = slot.Value;

            battler.SetMoves([.. moves]);
            return battler;
        }

        public PBattleMove CreateMove (string name)
        {
            var move = moveService.GetByName(name);
            if (move == null)
                throw new Exception($"Move '{name}' not found");

            return new PBattleMove(move);
        }
    }   
}