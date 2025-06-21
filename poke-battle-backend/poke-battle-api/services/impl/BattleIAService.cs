
using poke.battle.bridge.api;
using poke.battle.bridge.Api;

namespace poke_battle_api.services.impl
{
    public class BattleIAService(HttpClient client) : AbstractIAService, IBattleIAService
    {
        public override HttpClient Client { get; } = client;
        private static readonly string[] AVAILABLE_ACTIONS = { "move", "switch" };

        public async Task<BattleDecisionResponse> DecideAction(BattleDecisionRequest request)
        {
            var decision = await Client.PostAsJsonAsync("/api/gen-ai/battle/decide", request);
            var botResponse = await decision.Content.ReadFromJsonAsync<BattleDecisionResponse>();

            if (botResponse == null || !AVAILABLE_ACTIONS.Contains(botResponse.Action)  || string.IsNullOrEmpty(botResponse.Value))
                throw new Exception("FATAL: IA Could not response");
            return botResponse;
        }

        public async Task<TeamSelectionResponse> DecideTeam(TeamSelectionRequest request)
        {
            var decision = await Client.PostAsJsonAsync("/api/gen-ai/battle/select-team", request);
            var botResponse = await decision.Content.ReadFromJsonAsync<TeamSelectionResponse>();

            if (botResponse == null || botResponse.Team == null || botResponse.Team.Count == 0)
                throw new Exception("FATAL: IA Could not response");
            return botResponse;
        }
    }
}
