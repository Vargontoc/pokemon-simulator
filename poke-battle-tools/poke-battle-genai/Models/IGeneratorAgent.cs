using System.Text.Json.Nodes;

namespace poke.battle.genai.Models
{
    public interface IGeneratorAgent : IAgent
    {
        Task<JsonObject> CreateAbility();

    }
}
