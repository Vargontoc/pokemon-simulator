using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using poke.battle.bridge;
using poke.battle.bridge.api;
using poke.battle.bridge.Api;
using poke.battle.genai.Models.Agents;

namespace poke.battle.genai.Controllers
{
    [ApiController]
    [Route("api/gen-ai/battle")]
    public class BattleIAController(ILogger<BattleIAController> logger, IBattleAgent battleAgent) : ControllerBase
    {
        private readonly ILogger<BattleIAController> _logger = logger;
        private readonly IBattleAgent _battleAgent = battleAgent;
        // Define your actions here

        [HttpPost("decide")]
        public async Task<ActionResult<BattleDecisionResponse>> DecideAsync([FromBody] BattleDecisionRequest request)
        {
            var response = await _battleAgent.DecideAsync(request);
            return Ok(response);
        }

        [HttpPost("select-team")]
        public async Task<ActionResult<BattleDecisionResponse>> DecideTeamAsync([FromBody] TeamSelectionRequest request)
        {
            var response = await _battleAgent.DecideAsync(request);
            return Ok(response);
        }
    }
}
