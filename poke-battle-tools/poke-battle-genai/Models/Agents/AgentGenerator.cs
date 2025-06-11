
namespace poke.battle.genai.Models.Agents
{
    public class AgentGenerator : IAgent
    {


        /// <summary>
        /// Proceso de generación de texto a partir de una descripción.
        /// </summary>
        /// <param name="input">Descripcion introducida por el usuario</param>
        /// <returns></returns>
        public Task<string> InvokeAsync(ChatMessage input)
        {
            return Task.FromResult($"[Generado desde descripción]: {input}");
        }
    }
}
