
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using poke.battle.bridge.api;
using poke.battle.bridge.Api;
using System.Collections.Generic;
using System.Text.Json;

namespace poke.battle.genai.Models.Agents
{
    public class AgentBattle(Kernel kernel, ILogger<AgentBattle> logger) : IBattleAgent
    {
        private readonly Kernel _kernel = kernel;
        private readonly ILogger<AgentBattle> _logger = logger;
        private IChatCompletionService decicionSelectionChat;
        private IChatCompletionService teamSelectionChat;
        

        public async Task<BattleDecisionResponse> DecideAsync(BattleDecisionRequest context)
        {
            _logger.LogInformation("Decidiendo acción de batalla para el contexto: {Context}", context);
            var chat = _kernel.GetRequiredService<IChatCompletionService>();
            

            var history = new ChatHistory();
            history.AddSystemMessage(SYSTEM_MESSAGE);
            history.AddUserMessage(context.Prompt);

            var response = await chat.GetChatMessageContentAsync(history);
            var content = response?.Content?.Trim();
            if (string.IsNullOrEmpty(content)) {
                return new BattleDecisionResponse() { Value = "empty", Action = "unknown", Rationale = "empty" };
            }

            try
            {
                var result = JsonSerializer.Deserialize<BattleDecisionResponse>(content, new JsonSerializerOptions() { 
                    PropertyNameCaseInsensitive = true,
                });
                return result ?? new BattleDecisionResponse() { Value = content, Action = "unknown", Rationale = "empty" };
            }catch (JsonException ex)
            {
                return new BattleDecisionResponse() { Value = content ?? "", Action = "unknown", Rationale = $"Error al interpretar respuesta de la IA: {ex.Message}" };
            }


        }



        public Task<string> InvokeAsync(ChatMessage input)
        {
            return Task.FromResult($"[Sin respuesta del modelo]");
        }

        public async Task<TeamSelectionResponse> DecideAsync(TeamSelectionRequest request)
        {
            var prompt = request.ToPrompt();
            if(teamSelectionChat == null)
                teamSelectionChat = _kernel.GetRequiredService<IChatCompletionService>();
            var history = new ChatHistory();
            history.AddSystemMessage(TEAM_SELECTION_SYSTEM_PROMPT);
            history.AddUserMessage(prompt);

            var reply = await teamSelectionChat.GetChatMessageContentAsync(history);
            var raw = reply?.Content?.Trim();

            _logger.LogDebug("Respuesta cruda de la IA: {Raw}", raw);

            var json = ExtractJsonBlock(raw);

            try
            {
                var result = JsonSerializer.Deserialize<TeamSelectionResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result ?? new TeamSelectionResponse { Reason = "Respuesta vacía" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al deserializar respuesta");
                return new TeamSelectionResponse
                {
                    Team = new(),
                    Reason = "Error al interpretar respuesta: " + ex.Message
                };
            }
        }

        private string ExtractJsonBlock(string? input)
        {
            if (string.IsNullOrEmpty(input)) return "{}";
            int start = input.IndexOf('{');
            int end = input.LastIndexOf('}');
            return (start >= 0 && end > start) ? input[start..(end + 1)] : input;
        }

        const string SYSTEM_MESSAGE = @"""
Eres un agente de batalla Pokémon, eres un experto jugador. Tu tarea es analizar el estado del combate y tomar la mejor decisión posible. 

Tu respuesta debe ser **un único JSON válido** con esta estructura:

{
  ""rationale"": ""Explicación detallada de la decisión."",
  ""action"": ""move"", // o ""switch"",
  ""value"": ""NombreDelMovimientoOIdPokémon""
}

Devuelve solo ese JSON, sin texto adicional.
""";

        const string TEAM_SELECTION_SYSTEM_PROMPT = @"""
Eres un modelo de IA embebido en un servicio web. El frontend solo espera un JSON válido como respuesta. Todo texto fuera del JSON romperá el sistema.

❗ Solo puedes elegir Pokémon que estén explícitamente listados a continuación.

❌ Si eliges uno no presente en la lista, tu respuesta será ignorada.
❌ Si escribes cualquier texto fuera del JSON, tu respuesta será inválida.

Tu respuesta debe ser **un único JSON válido** con esta estructura:
{
  ""team"": [""pokemon1"",""pokemon2"" ...],
  ""reason"": ""Explicacion detallada de la seleccion""
}
Devuelve solo ese JSON, sin texto adicional.
""";


    }
}
