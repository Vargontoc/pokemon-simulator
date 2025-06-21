using poke.battle.bridge.api;
using poke.battle.bridge.Api;

namespace poke_battle_api.services
{
    public interface IBattleIAService : IAIService
    {
        Task<BattleDecisionResponse> DecideAction(BattleDecisionRequest request);
        Task<TeamSelectionResponse> DecideTeam(TeamSelectionRequest request);
    }
}
