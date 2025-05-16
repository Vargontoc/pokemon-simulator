using Microsoft.AspNetCore.Mvc;
using poke.battle.Models;
using poke.battle.Models.Impl;
using poke.battle.services;
using poke_battle_api.mappers;
using poke_battle_api.utils;

namespace poke_battle_api.controllers {

    [ApiController]
    [Route("api/combos")]
    public class CombosController(ITypeService service) : Controller
    {

        [HttpGet("types")]
        public IActionResult GetComboTypes()
        {
            var result = service.FindAll(null!, null!);

            return Ok(new Combo()
            {
                Items = result.Results.Select(x => new ComboItem() { Key = x.Name, Value = x.DisplayName, Icon = x.Icon }).ToList()
            });
        }

        [HttpGet("move-categories")]
        public IActionResult GetComboCategoryMoves() 
        {
            List<ComboItem> items = [];
            foreach(var v in Enum.GetValues<MoveType>())
            {
                items.Add(new()
                {
                    Key = v.ToString(),
                    Value = v.ToString(),
                });
            }
            return Ok(new Combo() { Items = items });
        }

        [HttpGet("growths")]
        public IActionResult GetComboGrowth()
        {
            List<ComboItem> items = [];
            foreach(var v in Growth.GetValues())
            {
                items.Add(new()
                {
                    Key = v.Code,
                    Value = v.Name
                });
            }

            return Ok(new Combo() { Items = items });
        }
    }
}

