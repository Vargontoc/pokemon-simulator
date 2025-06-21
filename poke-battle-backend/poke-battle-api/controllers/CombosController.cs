using Microsoft.AspNetCore.Mvc;
using poke.battle.core;
using poke.battle.Models;
using poke.battle.Models.Impl;
using poke.battle.services;
using poke_battle_api.utils;

namespace poke_battle_api.controllers {

    [ApiController]
    [Route("api/combos")]
    public class CombosController(ITypeService service, IMoveService moveService, IPokemonService pokeService, IAbilityService abilityService) : Controller
    {
        [HttpGet("species")]
        public IActionResult GetComboSpecies()

        {
            var result = pokeService.FindAll(null!, null!);
            return Ok(new Combo()
            {
                Items = [.. result.Results.Where(z => !string.IsNullOrEmpty(z.DisplayName)).Select(x => new ComboItem() { Key = x.Name, Value = x.DisplayName }).OrderBy(y => y.Value)]
            });
        }


        [HttpGet("abilities")]
        public IActionResult GetComboAbilities()

        {
            var result = abilityService.FindAll(null!, null!);
            return Ok(new Combo()
            {
                Items = [.. result.Results.Where(z => !string.IsNullOrEmpty(z.DisplayName)).Select(x => new ComboItem() { Key = x.Name, Value = x.DisplayName }).OrderBy(y => y.Value)]
            });
        }

        [HttpGet("types")]
        public IActionResult GetComboTypes()
        {
            var result = service.FindAll(null!, null!);

            return Ok(new Combo()
            {
                Items = [.. result.Results.Select(x => new ComboItem() { Key = x.Name, Value = x.DisplayName, Icon = x.Icon })]
            });
        }

        [HttpGet("moves")]
        public IActionResult GetComboMoves()
        {
            var result = moveService.FindAll(null!, null!);

            return Ok(new Combo()
            {
                Items = [.. result.Results.Select(x => new ComboItem() { Key = x.Name, Value = x.DisplayName })]
            });
        }

        [HttpGet("move-categories")]
        public IActionResult GetComboCategoryMoves()
        {
            List<ComboItem> items = [];
            foreach (var v in Enum.GetValues<MoveType>())
            {
                items.Add(new()
                {
                    Key = v.ToString(),
                    Value = v.ToString(),
                });
            }
            return Ok(new Combo() { Items = items });
        }

        [HttpGet("stats")]
        public IActionResult GetStats()
        {
            List<Tuple<byte, string>> stats = [];
            List<ComboItem> items = [];
            foreach (var v in Stat.GetValues().OrderBy(x => x.Index))
            {
                stats.Add(new Tuple<byte, string>(v.Index, v.Abbr));
            }
            return Ok(stats);
        }

        [HttpGet("growths")]
        public IActionResult GetComboGrowth()
        {
            List<ComboItem> items = [];
            foreach (var v in Growth.GetValues())
            {
                items.Add(new()
                {
                    Key = v.Code,
                    Value = v.Name
                });
            }

            return Ok(new Combo() { Items = items });
        }

        [HttpGet("natures")]
        public IActionResult GetComboNatures()
        {
            var result = Nature.GetValues();
            return Ok(new Combo()
            {
                Items = [.. result.Select(x => new ComboItem() { Key = x.Name, Value = x.Name })]
            });
        }
    }
}

