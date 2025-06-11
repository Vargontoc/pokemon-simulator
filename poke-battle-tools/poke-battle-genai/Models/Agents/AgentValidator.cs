
namespace poke.battle.genai.Models.Agents
{
    public class AgentValidator : IAgent
    {
        /// <summary>
        /// Proceso de validación de datos introducidos por el usuario.
        /// </summary>
        /// <param name="input">Datos introducidos por el usuario</param>
        /// <returns></returns>
        public Task<string> InvokeAsync(ChatMessage input)
        {
            return Task.FromResult($"[Validación]: Los datos son correctos para: {input}");
        }
    }
}
