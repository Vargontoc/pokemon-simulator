using Microsoft.AspNetCore.Mvc;
using poke.battle.core;
using poke.battle.factories;
using poke.battle.services;
using poke_battle_api.mappers.impl;

namespace poke_battle_api.controllers
{
    [ApiController]
    [Route("api/simulator")]
    public class SimulatorController : Controller
    {
        private readonly BattlerFactory battlerFactory;

        public SimulatorController(IAbilityService abilities, IMoveService moves, IPokemonService species) => battlerFactory = new BattlerFactory(species, moves);

        [HttpGet("start")]
        public IActionResult StartBattle() 
        {
            var player = battlerFactory.CreateBattler("pikachu", 50, string.Empty);
            var enemy = battlerFactory.CreateBattler("bulbasaur", 50, string.Empty);
            return Ok(
                new
                {
                    player = new[] { PBattlerMapper.ConvertTo(player) },
                    enemy = new[] { PBattlerMapper.ConvertTo(enemy) },
                }
            );
        }
    }
}
