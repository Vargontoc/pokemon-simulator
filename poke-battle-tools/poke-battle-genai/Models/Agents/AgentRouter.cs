namespace poke.battle.genai.Models.Agents
{
    public class AgentRouter : IAgentRouter
    {
        private readonly Dictionary<string, IAgent> _agents = [];
        public AgentRouter(IServiceProvider sp)
        {
            _agents = new Dictionary<string, IAgent>(StringComparer.OrdinalIgnoreCase)
            {
                ["battle"] = sp.GetRequiredService<AgentBattle>(),
                ["chat"] = sp.GetRequiredService<AgentChat>(),
                ["generator"] = sp.GetRequiredService<AgentGenerator>(),
                ["narrator"] = sp.GetRequiredService<AgentNarrator>(),
                ["strategy"] = sp.GetRequiredService<AgentStrategy>(),
                ["translator"] = sp.GetRequiredService<AgentTranslator>(),
                ["validation"] = sp.GetRequiredService<AgentValidator>()
            };
            
        }
        public IAgent GetAgent(string name)
        {

            return _agents.TryGetValue(name, out var agent) ? agent : throw new ArgumentException($"Agent '{name}' not found.");
        }
    }
    
}
