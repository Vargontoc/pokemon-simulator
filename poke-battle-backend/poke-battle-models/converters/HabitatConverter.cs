using Newtonsoft.Json;
using poke.battle.Models.Impl;

namespace poke.battle.Models.converters
{
    public class HabitatConverter : JsonConverter<Habitat>
    {
        public override Habitat? ReadJson(JsonReader reader, Type objectType, Habitat? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string? code = reader.Value?.ToString();
            return code != null ? Habitat.Get(code) : Habitat.Unknown;
        }

        public override void WriteJson(JsonWriter writer, Habitat? value, JsonSerializer serializer)
        {
            if(value != null)
                writer.WriteValue(value.Code);
            else
                writer.WriteNull();
        }
    }
}