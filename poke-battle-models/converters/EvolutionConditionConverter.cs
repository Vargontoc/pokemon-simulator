using Newtonsoft.Json;
using poke.battle.Models.Impl;

namespace poke.battle.Models.converters
{
    public class EvolutionConditionConverter : JsonConverter<EvolutionCondition>
    {
        public override EvolutionCondition? ReadJson(JsonReader reader, Type objectType, EvolutionCondition? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string? code = reader.Value?.ToString();
            return code != null ? EvolutionCondition.Get(code) : EvolutionCondition.Level;
        }

        public override void WriteJson(JsonWriter writer, EvolutionCondition? value, JsonSerializer serializer)
        {
            if(value != null)
                writer.WriteValue(value.Code);
            else
                writer.WriteNull();
        }
    }
}