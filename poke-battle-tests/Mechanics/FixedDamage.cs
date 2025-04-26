using poke.battle.core;
using poke.battle.factories;
using poke.battle.services.impl;
using poke.battle.services;
using poke.battle.Models.helpers;
using poke.battle.infraestructure.repositories.impl;

namespace poke_battle_tests.Mechanics
{
    
    public class FixedDamage
    {
        [Fact]
        public void DragonRage_DealsFixedDamage()
        {
            ITypeService typeService = new TypesService(new TypesRepository());
            TypeEffectivenessResolver.Initialize(typeService.FindAll(null!, null!).Results);
            IPokemonService specieService = new PokemonsService(new PokemonsRepository());
            IMoveService moveService = new MovesService(new MovesRepository());

            var factory = new BattlerFactory(specieService, moveService);
            var attacker = factory.CreateBattler("bulbasaur", 50, null);
            attacker.IsActive = true;
            var defender = factory.CreateBattler("charmander", 50, null);
            defender.IsActive = true;

            var dragonRage = factory.CreateMove("dragon-rage");
            dragonRage.Move.MoveEffect = MoveEffect.Fixed_40;
            var context = new BattleContext([attacker], [defender]);
            
            var action = new MoveAction() { Actor = attacker, Move = dragonRage, Context = context };
            var result = action.Execute(defender);

            Assert.Contains(result.Events, e => e.Type == "damage");
            Assert.Equal(40, result.LastDamage);
            Assert.Equal(defender.GetStat(Stat.Hp).RawValue - 40, defender.CurrentHp);
        }

        [Fact]
        public void SeismicToss_DealsFixedDamage()
        {
            ITypeService typeService = new TypesService(new TypesRepository());
            TypeEffectivenessResolver.Initialize(typeService.FindAll(null!, null!).Results);
            IPokemonService specieService = new PokemonsService(new PokemonsRepository());
            IMoveService moveService = new MovesService(new MovesRepository());

            var factory = new BattlerFactory(specieService, moveService);
            var attacker = factory.CreateBattler("bulbasaur", 50, null);
            attacker.IsActive = true;
            var defender = factory.CreateBattler("charmander", 50, null);
            defender.IsActive = true;

            var dragonRage = factory.CreateMove("seismic-toss");
            dragonRage.Move.MoveEffect = MoveEffect.LevelDamage;
            var context = new BattleContext([attacker], [defender]);

            var action = new MoveAction() { Actor = attacker, Move = dragonRage, Context = context };
            var result = action.Execute(defender);

            Assert.Contains(result.Events, e => e.Type == "damage");
            Assert.Equal(attacker.Level, result.LastDamage);
            Assert.Equal(defender.GetStat(Stat.Hp).RawValue - attacker.Level, defender.CurrentHp);
        }
    }
}
