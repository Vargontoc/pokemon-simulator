using Microsoft.AspNetCore.Mvc;
using poke.battle.core;
using poke.battle.services;

namespace poke_battle_api.controllers
{
    public class BaseController : Controller
    {
        protected BaseController(IAbilityService abilities, IMoveService moves, IPokemonService species)
        {
            CoreSettings.InitCore(abilities.FindAll(null!, null!).Results, moves.FindAll(null!, null!).Results, species.FindAll(null!, null!).Results);
        }
    }
}
