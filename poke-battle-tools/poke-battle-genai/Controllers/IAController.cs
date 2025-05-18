using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Text.Json;

namespace poke_battle_genai.Controllers
{
    

    [ApiController]
    [Route("api/gent-ai")]
    public class IAController : Controller
    {
        readonly ChatHistory chatHistory = new ChatHistory();
        readonly ChatCompletionAgent agent;

        public IAController(): base()
        {
            var aibuilder = Kernel.CreateBuilder();

            // Suppress the diagnostic warning SKEXP0070 by explicitly acknowledging the experimental nature of the API
#pragma warning disable SKEXP0070
            aibuilder.AddOllamaChatCompletion("llama3", new Uri("http://localhost:11434"));
#pragma warning restore SKEXP0070
            var kernel = aibuilder.Build();

            agent = new()
            {
                Instructions = "Eres un experto en Pokemon, tanto en su biologia como entrenamiento. Todas las respuesta en español con las curisodades tambien",
                Name = "Profesor Oak",
                Kernel = kernel,

            };

        }

        public async Task<IActionResult> ChatNNpc()
        {
            chatHistory.AddMessage(AuthorRole.User, "Dame una curiosidad aleatoria de un pokemon");
            string content = string.Empty;
            await foreach(var response in agent.InvokeAsync(chatHistory))
            {
                chatHistory.Add(response);
                content = response.Content!;
            }
            return Ok(new { content });
        }
    }
}
