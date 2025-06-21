using poke.battle.Models.Impl;
using System.Text.Json.Nodes;

namespace poke_battle_api.services
{
    public interface IGeneratorAIService
    {
        Task<AbilityModel> GenerateAbility();
    }
}
