using Microsoft.SemanticKernel;

namespace poke.battle.genai.Models
{
    public interface IAgent
    {
        Task<string> InvokeAsync(ChatMessage input);
    }
}
