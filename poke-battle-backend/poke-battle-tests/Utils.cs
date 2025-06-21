using poke.battle.core;
using poke.battle.factories;
using poke.battle.infraestructure.repositories.impl;
using poke.battle.services;
using poke.battle.services.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace poke_battle_tests
{
    public static class Utils
    {
        private static BattlerFactory? _factory;

        private static void Initialize()
        {
            if(_factory == null)
            {
                ITypeService typeService = new TypesService(new TypesRepository(), null!, null!, null!);
                TypeEffectivenessResolver.Initialize(typeService.FindAll(null!, null!).Results);
                IPokemonService pokeService = new PokemonsService(new PokemonsRepository());
                IMoveService moveService = new MovesService(new MovesRepository());

                _factory = new BattlerFactory(pokeService, moveService);
            }
        }

        public static PBattler GetBattler(string name, int level)
        {
            Initialize();
            return _factory!.CreateBattler(name, level, null);
        }

        public static PBattleMove GetMove(string name)
        {
            Initialize();
            return _factory!.CreateMove(name);
        }
    }
}
