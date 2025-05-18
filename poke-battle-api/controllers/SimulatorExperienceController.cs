using Microsoft.AspNetCore.Mvc;
using poke.battle.core;
using poke.battle.Models.Impl;
using poke_battle_api.dtos;

namespace poke_battle_api.controllers
{
    [ApiController]
    [Route("api/simulator/experience")]
    public class SimulatorExperienceController : Controller
    {
        [HttpPost("growth")]
        public IActionResult CalculateExp([FromBody] CalcExpDto exp)
        {
            Growth g = Growth.Get(exp.code);
            if(g == null || exp.level <= 0 || exp.level > CoreSettings.MAX_LEVEL)
            {
                return BadRequest();
            }

            return Ok(Calculator.GetExp(g, exp.level));
        }

        [HttpGet("graph/{code}")]
        public IActionResult GetGraphData(string code)
        {
            Growth g = Growth.Get(code);
            if(g == null)
            {
                return BadRequest();
            }

            return Ok(Calculator.GetExperienceValues(g));
        }
    }
}
