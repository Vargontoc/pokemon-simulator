using poke.battle.Models;
using poke.battle.Models.Impl;

namespace poke.battle.core 
{
    public static class CoreSettings
    {
        public const string MOD_ACC = "accuracy";
        public const string MOD_EV = "evassion";
        public const int MAX_LEVEL = 100;
        private static readonly Random rng = new Random();

        private static IEnumerable<AbilityModel> _abilities = [];
        private static IEnumerable<MoveModel> _moves = [];
        private static IEnumerable<SpecieModel> _species = [];

        private static bool _initialize = false;
        public static void InitCore(IEnumerable<AbilityModel> abilities, IEnumerable<MoveModel> moves, IEnumerable<SpecieModel> species)
        {
            if (!_initialize)
            {
                _abilities = abilities;
                _moves = moves;
                _species = species;
                _initialize = true;
            }
        }

        public static MoveModel GetRandomMove(params string[] movesExcludes)
        {
            var availableMoves = _moves.Where(x => !movesExcludes.Contains(x.Name)).ToList();
            return availableMoves[rng.Next(availableMoves.Count)];
        }

        public static AbilityModel GetAbility(string name)
        {
            if (_abilities != null)
                return _abilities.FirstOrDefault(x => x.Name == name) ?? null!;

            return null!;
        }

        public static MoveModel GetMove(string name) 
        {
            if (_moves != null)
                return _moves.FirstOrDefault(x => x.Name == name) ?? null!;

            return null!;
        }

        public static SpecieModel GetSpecie(string name)
        {
            if (_species != null)
                return _species.FirstOrDefault(x => x.Name == name) ?? null!;

            return null!;

        }

    }
}