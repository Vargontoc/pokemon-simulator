using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel.Agents;
using poke.battle.genai.Models.Agents;

namespace poke.battle.genai.Controllers
{

    [ApiController]
    [Route("api/generator")]
    public class GeneratorController(AgentGenerator agent) : Controller
    {
        [HttpGet("ability")]
        public async Task<IActionResult> GenerateAbility()
        {
            try
            {
                var response = await agent.CreateAbility();
                if (response == null)
                    return BadRequest(response);

                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
