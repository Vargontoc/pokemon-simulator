
namespace poke.battle.genai.Models.Agents
{
    public class AgentStrategy : IAgent
    {
        public Task<string> InvokeAsync(ChatMessage input)
        {
            return Task.FromResult($"[Consejo]: Lo mejor seria usar Reflejo ahora.");

        }
    }
}
