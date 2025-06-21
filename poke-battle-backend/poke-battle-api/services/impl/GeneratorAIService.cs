using poke.battle.Models.Impl;
using System.Text.Json.Nodes;

namespace poke_battle_api.services.impl
{
    public class GeneratorAIService(HttpClient client) : AbstractIAService, IGeneratorAIService
    {
        public override HttpClient Client { get; } = client;

        public async Task<AbilityModel> GenerateAbility()
        {
            var decision = await Client.GetAsync("/api/generator/ability");
            var botResponse = await decision.Content.ReadFromJsonAsync<AbilityModel>();
            
            if(botResponse == null)
                throw new Exception("Failed to generate ability, response was null.");
            return botResponse;
        }
    }
}
