
namespace poke.battle.genai.Models.Agents
{
    public class AgentNarrator : IAgent
    {
        public Task<string> InvokeAsync(ChatMessage input)
        {
            return Task.FromResult($"[Narrador]: {input} ¡Ha sido muy efectivo!");
        }
    }
}
