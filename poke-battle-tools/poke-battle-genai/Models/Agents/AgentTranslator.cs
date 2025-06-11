using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace poke.battle.genai.Models.Agents
{
    /// <summary>
    /// Agente de traducción de texto.
    /// </summary>
    /// <param name="kernel"></param>
    /// <param name="name"></param>
    /// <param name="systemPrompt"></param>
    public class AgentTranslator(Kernel kernel) : IAgent
    {
        private readonly Kernel _kernel = kernel;

        /// <summary>
        /// Traduce un texto de un idioma a otro.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<string> InvokeAsync(ChatMessage input)
        {
            string systemPrompt = $"Eres un traductor de texto. Traduce el texto al idioma '{input.language}' de forma precisa y natural. Solo responde con la traducción, sin explicaciones y ningún texto extra. Si no se puede traducir o no sabes, no inventes y devuelve el texto original";
            if (input.context.Equals("pokemon", StringComparison.InvariantCultureIgnoreCase))
            {
                systemPrompt = $"Eres un traductor de texto. Traduce Traduce al idioma '{input.language}' una frase del universo Pokémon (movimientos, habilidades, efectos). Usa la terminología oficial si existe.";
            }
           
            var chat = new ChatHistory();
            chat.AddSystemMessage(systemPrompt);
            chat.AddUserMessage(input.text);

            var chatService = _kernel.GetRequiredService<IChatCompletionService>();
            var response = await chatService.GetChatMessageContentAsync(chat);

            return response?.Content ?? string.Empty;
        }  
    }
}
