using Newtonsoft.Json;
using poke.battle.Models.Impl;

namespace poke.battle.Models.converters
{
    public class GrowthConverter : JsonConverter<Growth>
    {
        public override Growth? ReadJson(JsonReader reader, Type objectType, Growth? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string? code = reader.Value?.ToString();
            return code != null ? Growth.Get(code) : Growth.Medium;
        }

        public override void WriteJson(JsonWriter writer, Growth? value, JsonSerializer serializer)
        {
            if(value != null)
                writer.WriteValue(value.Code);
            else
                writer.WriteNull();
        }
    }
}