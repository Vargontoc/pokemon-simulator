namespace poke.battle.genai.Models
{
    public interface IAgentRouter
    {
        IAgent GetAgent(string name); 
    }
}
