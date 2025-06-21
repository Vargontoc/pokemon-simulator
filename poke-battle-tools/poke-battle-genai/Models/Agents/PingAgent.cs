
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace poke.battle.genai.Models.Agents
{
    public class PingAgent(Kernel kernel, ILogger<PingAgent> logger) : IAgent
    {
        public async Task<string> InvokeAsync(ChatMessage input)
        {
            try
            {
                logger.LogInformation("Realizando ping al servidor Ollama");
                var chat = kernel.GetRequiredService<IChatCompletionService>();
                var history = new ChatHistory();
                history.AddSystemMessage(SYSTEM_MESSAGE);
                history.AddUserMessage("devuelve un texto myt corto");

                var response = await chat.GetChatMessageContentAsync(history);
                logger.LogInformation("Respues de Ollama: {Resp}", response?.Content);

                return response?.Content!;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error al hacer ping al servidor Ollama");
                return string.Empty;
            }
        }



        const string SYSTEM_MESSAGE = @"""
Eres un agente básico, solo tienes que escribir el numero 1, sin dar detalles ni nada. simpemente 1
""";
    }
}
