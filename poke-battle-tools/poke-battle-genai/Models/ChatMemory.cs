namespace poke.battle.genai.Models
{
    public class ChatMemory
    {
        public List<ChatTurn> Messages { get; set; } = new List<ChatTurn>();
    }
}
