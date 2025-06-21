using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using poke.battle.genai.Models;
using poke.battle.genai.Models.Agents;
using System.Runtime.InteropServices;

namespace poke.battle.genai.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController(PingAgent agent) : Controller
    {
        [HttpGet("health")]
        public async Task<IActionResult> CheckHealth()
        {
            try
            {
                var response = await agent.InvokeAsync(null!);
                if(string.IsNullOrEmpty(response))
                    return BadRequest(response);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
