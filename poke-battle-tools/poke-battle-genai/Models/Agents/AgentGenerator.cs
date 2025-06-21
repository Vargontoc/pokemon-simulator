
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using poke.battle.bridge.Api;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace poke.battle.genai.Models.Agents
{
    public class AgentGenerator(Kernel kernel) : IGeneratorAgent
    {
        public async Task<JsonObject> CreateAbility()
        {
            var chat = kernel.GetRequiredService<IChatCompletionService>();
            var history = new ChatHistory();
            history.AddSystemMessage(SYSTEM_ABILITY_MESSAGE);
            history.AddUserMessage("Crea una habilidad de ejemplo para un Pokémon, con nombre, nombre para mostrar y descripción. En un fichero JSON como te he solicitado.");

            var reply = await chat.GetChatMessageContentAsync(history);
            var raw = reply?.Content?.Trim();


            try
            {
                var result = JsonSerializer.Deserialize<JsonObject>(raw, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result!;
            }
            catch (Exception ex)
            {
                return null!;
            }
        }

        /// <summary>
        /// Proceso de generación de texto a partir de una descripción.
        /// </summary>
        /// <param name="input">Descripcion introducida por el usuario</param>
        /// <returns></returns>
        public Task<string> InvokeAsync(ChatMessage input)
        {
            return Task.FromResult($"[Generado desde descripción]: {input}");
        }

        const string SYSTEM_ABILITY_MESSAGE = @"""
Eres un agente experto en pokemon, Tu tarea es generar una habilidad Pokemon. 

Tu respuesta debe ser **un único JSON válido** con esta estructura:

{
  ""name"": ""nombre de la habilidad en minusculas"",
  ""displayname"": ""nombre que ve el usuario"",
  ""description"": ""descripcion de la habilidad""
}

Devuelve solo ese JSON, sin texto adicional.
""";
    }
}
