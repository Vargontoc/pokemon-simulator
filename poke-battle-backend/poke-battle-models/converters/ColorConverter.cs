using Newtonsoft.Json;
using poke.battle.Models.Impl;

namespace poke.battle.Models.converters
{
    public class ColorConverter : JsonConverter<Color>
    {
        public override Color? ReadJson(JsonReader reader, Type objectType, Color? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string? code = reader.Value?.ToString();
            return code != null ? Color.Get(code) : Color.Black;
        }

        public override void WriteJson(JsonWriter writer, Color? value, JsonSerializer serializer)
        {
            if(value != null)
                writer.WriteValue(value.Code);
            else
                writer.WriteNull();
        }
    }
}