using Microsoft.SemanticKernel.ChatCompletion;

namespace poke_battle_genai.Models
{
    public static class MemoryStore
    {
        private static readonly Dictionary<string, ChatHistory> _memory = new();
        public static ChatHistory GetOrCreate(string key)
        {
            if (!_memory.ContainsKey(key))
            {
                _memory[key] = new();
            }
            return _memory[key];
        }

    }
}
