using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using poke_battle_genai.Models;
using poke_battle_genai.ModelViews;
using System.Text.Json;

namespace poke_battle_genai.Controllers
{
    

    [ApiController]
    [Route("api/gen-ai")]
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
                Instructions = "Eres el Profesor Oak, un experto en Pokémon tanto en su lore como competitivo. \r\nSiempre responde en **español**. \r\n\r\n⚠️ Tu respuesta debe estar **formateada exclusivamente en HTML válido**, usando etiquetas como `<p>`, `<ul>`, `<li>`, `<strong>`, `<h3>`, etc.\r\n\r\n❌ No incluyas texto plano sin etiquetas HTML.\r\n\r\n✅ Ejemplo de respuesta correcta:\r\n<h3>¡Hola, entrenador!</h3>\r\n<p>Aquí tienes información útil:</p>\r\n<ul>\r\n  <li><strong>Pikachu</strong> es un Pokémon tipo eléctrico.</li>\r\n  <li><strong>Charizard</strong> es fuego/volador.</li>\r\n</ul>\r\n\r\nSi te hacen una pregunta, estructura tu respuesta en HTML como el ejemplo.",
                Name = "Profesor Oak",
                Kernel = kernel,

            };

        }

        [HttpPost("talk")]
        public async Task<IActionResult> ChatNNpc([FromBody] ChatBotMessage msg)
        {
            if(string.IsNullOrEmpty(msg.Prompt)) return BadRequest("El mensaje no puede estar vacio");

            var chatHistory = MemoryStore.GetOrCreate("profesor-oak");
            chatHistory.AddUserMessage(msg.Prompt);
            string content = string.Empty;
            await foreach (var response in agent.InvokeAsync(chatHistory))
            {
                chatHistory.Add(response);
                content = response.Content!;
            }
           
            return Ok(new { content });
        }
    }
}
