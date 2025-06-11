namespace poke.battle.genai.Models
{
    public record ChatMessage(string text, string agent, string context = "",   string language = "es")
    {
    }
}
