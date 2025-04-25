using Microsoft.AspNetCore.Mvc;
using poke.battle.services;
using poke_battle_api.mappers;
using poke_battle_api.utils;

namespace poke_battle_api.controllers {

    [ApiController]
    [Route("api/combos")]
    public class CombosController(ITypeService service) : Controller
    {

        [HttpGet("types")]
        public IActionResult GetComboTypes(int id)
        {
            var result = service.FindAll(null!, null!);

            return Ok(new Combo()
            {
                Items = result.Results.Select(x => new ComboItem() { Key = x.Name, Value = x.DisplayName, Icon = x.Icon }).ToList()
            });
        }
    }
}

