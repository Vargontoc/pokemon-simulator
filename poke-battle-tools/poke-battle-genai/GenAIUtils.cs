using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace poke.battle.genai
{
    public class GenAIUtils
    {
        public static async Task CheckHealth(ILogger logger, Kernel kernel)
        {
            await CheckOllamaServer(logger, kernel);
        }

        private  static async Task CheckOllamaServer(ILogger logger, Kernel kernel)
        {
            try
            {
                logger.LogInformation("Realizando ping al servidor Ollama");
                var chat = kernel.GetRequiredService<IChatCompletionService>();
                var history = new ChatHistory();
                history.AddUserMessage("ping");

                var response = await chat.GetChatMessageContentAsync(history);
                logger.LogInformation("Respues de Ollama: {Resp}", response?.Content);

            }catch(Exception e)
            {
                logger.LogError(e, "Error al hacer ping al servidor Ollama");
            }
        }
     }
}
