using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using poke.battle.Models.Impl;

public class EggGroupConverter : JsonConverter<EggGroup>
{
    public override EggGroup? ReadJson(JsonReader reader, Type objectType, EggGroup? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        string? code = reader.Value?.ToString();
        return code != null ? EggGroup.Get(code) : null;
    }

    public override void WriteJson(JsonWriter writer, EggGroup? value, JsonSerializer serializer)
    {
        if(value != null)
            writer.WriteValue(value.Code);
        else
            writer.WriteNull();
    }
}

public class EggGroupArrayConverter : JsonConverter<EggGroup[]>
{
    public override EggGroup[]? ReadJson(JsonReader reader, Type objectType, EggGroup[]? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var token = JToken.Load(reader);
        if(token.Type != JTokenType.Array)
            return [];
        
        var codes = token.ToObject<string[]>();
        if(codes == null || codes.Length == 0)
            return  [];
        return codes.Select(eg => EggGroup.Get(eg)).ToArray();
    }

    public override void WriteJson(JsonWriter writer, EggGroup[]? value, JsonSerializer serializer)
    {
        if(value == null)
        {
            writer.WriteNull();
            return;
        }

        writer.WriteStartArray();
        value.ToList().ForEach(eg => writer.WriteValue(eg.Code));
        writer.WriteEndArray();
    }
}