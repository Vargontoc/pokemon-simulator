using poke.battle.bridge.api;
using poke.battle.bridge.Api;
namespace poke.battle.genai.Models.Agents
{
    public interface IBattleAgent : IAgent
    {
        Task<BattleDecisionResponse> DecideAsync(BattleDecisionRequest context);

        Task<TeamSelectionResponse> DecideAsync(TeamSelectionRequest request);
    }
}
