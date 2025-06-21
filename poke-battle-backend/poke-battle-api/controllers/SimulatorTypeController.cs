using Microsoft.AspNetCore.Mvc;
using poke.battle.core;
using poke.battle.Models.Impl;
using poke.battle.services;
using poke_battle_api.mappers.impl;
using poke_battle_api.utils;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace poke_battle_api.controllers
{
    [ApiController]
    [Route("api/simulator/type")]
    public class SimulatorTypeController(ITypeService service) :Controller
    {
        [HttpPost("defender")]
        public ActionResult<Dictionary<double, List<object>>> GetMultiplierOnDefense([FromBody] string[] types)
        {
            if(types == null || types.Length <= 0 || types.Length > 2)
                return Ok(new Dictionary<double, List<object>>());

            var distinctTypes = service.FindAll(null!, null!).Results;

            Dictionary<double, List<object>> multipliers = [];
            foreach (var type in distinctTypes)
            {
                FillDictionary(multipliers, type, TypeEffectivenessResolver.GetEffectivenessAgainst(type.Name, types));
            }

            return Ok(multipliers);
            
        }

        [HttpPost("attacker")]
        public ActionResult<Dictionary<double, List<object>>> GetMultiplierOnAttack([FromBody] string[] type)
        {
            if(type == null || type.Length != 1)
                return Ok(new Dictionary<double, List<object>>());

            var distinctTypes = service.FindAll(null!, null!).Results;
            Dictionary<double, List<object>> multipliers = [];
            foreach (var typeModel in distinctTypes)
            {
                FillDictionary(multipliers, typeModel, TypeEffectivenessResolver.GetEffectiveness(type[0], typeModel.Name));

            }

            return Ok(multipliers);
        }


        private void FillDictionary(Dictionary<double, List<object>> multipliers, TypeModel type,  double eff)
        {
            if (!multipliers.TryGetValue(eff, out List<object>? value))
            {
                value = [];
                multipliers.Add(eff, value);
            }

            var item = new
            {
                key = type.Name,
                value = type.DisplayName,
                icon = type.Icon
            };
            value.Add(item);
        }

    }
}
