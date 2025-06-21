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
            Growth g = Growth.Get(exp.Code);
            if(g == null || exp.Level <= 0 || exp.Level > CoreSettings.MAX_LEVEL)
            {
                return Ok(0);
            }

            return Ok(Calculator.GetExp(g, exp.Level));
        }

        [HttpPost("gained")]
        public IActionResult CalculateExpGained(CalcExpDto exp)
        {
            return Ok(Calculator.CalculateExpGainer(exp.Level, exp.BaseExperience, exp.Participants, exp.LuckyEgg, exp.Trainer));
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
