using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace poke.battle.services
{
    public class OpenAIService
    {
        private readonly HttpClient _client;
        private readonly string _apiKey;
        public OpenAIService(string apiKey) 
        {
            _apiKey = apiKey;
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        public async Task<TranslationResult?> Translate(string prompt)
        {

        var request = new
        {
            model = "gpt-4",
            messages = new[]
            {
                new { role = "user", content = prompt }
            },
            temperature = 0.3
        };

        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("https://api.openai.com/v1/chat/completions", content);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error en la API de OpenAI: {response.StatusCode}");
            return null;
        }

        var json = await response.Content.ReadAsStringAsync();
        dynamic result = JsonConvert.DeserializeObject(json)!;
        string message = result.choices[0].message.content;

        try
        {
            // Intenta deserializar directamente del mensaje (debe estar en JSON si seguimos el prompt)
            var translation = JsonConvert.DeserializeObject<TranslationResult>(message);
            return translation;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error interpretando la respuesta: {ex.Message}");
            Console.WriteLine($"Contenido recibido: {message}");
            return null;
        }
        }
    }

    public class TranslationResult 
    {
        public string DisplayName {get; set;} = string.Empty;
        public string Abbr { get; set; } = string.Empty; 
        public string Description  { get; set; } = string.Empty;
    }
}