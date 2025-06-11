namespace poke.battle.genai.Models.Agents
{
    public class AgentChat :IAgent
    {
        public Task<string> InvokeAsync(ChatMessage input)
        {
            return Task.FromResult($"[Profesor Oak]: {input} ¡Hola!");
        }
    }
}
