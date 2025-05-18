using Newtonsoft.Json;
using poke.battle.Models.Impl;

namespace poke.battle.Models.converters
{
    public class EvolutionTriggerConverter : JsonConverter<EvolutionTrigger>
    {
        public override EvolutionTrigger? ReadJson(JsonReader reader, Type objectType, EvolutionTrigger? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string? code = reader.Value?.ToString();
            return code != null ? EvolutionTrigger.Get(code) : EvolutionTrigger.LevelUp;
        }

        public override void WriteJson(JsonWriter writer, EvolutionTrigger? value, JsonSerializer serializer)
        {
            if(value != null)
                writer.WriteValue(value.Code);
            else
                writer.WriteNull();
        }
    }
}