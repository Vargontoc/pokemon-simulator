using Microsoft.AspNetCore.Mvc;
using poke.battle.genai.Models;

namespace poke.battle.genai.Controllers
{
    [ApiController]
    [Route("api/gen-ai/crud")]
    public class AdminController(IAgentRouter router, ChatMemory memory) : Controller
    {
        private readonly IAgentRouter _agentRouter = router;
        private readonly ChatMemory _chatMemory = memory;

        [HttpPost("translator")]
        public async Task<IActionResult> Translate([FromBody] ChatMessage message)
        {
            var agent = _agentRouter.GetAgent(message.agent);
            _chatMemory.Messages.Add(new()
            {
                Role = "user",
                Content = message.text
            });

            string response = await agent.InvokeAsync(message);
            _chatMemory.Messages.Add(new()
            {
                Role = "agent",
                Content = response
            });
            // Logic to add a new translator
            return Ok(new { response });
        }
    }
}
