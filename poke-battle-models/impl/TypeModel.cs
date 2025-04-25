using Newtonsoft.Json;

namespace poke.battle.Models.Impl 
{
    public class TypeModel : IModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; } = string.Empty;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DisplayName { get; set; } = string.Empty;
        public int Id { get; set; } = 0;
        public string[] Resistences {get; set; } = [];
        public string[] Weakness { get; set; } = [];
        public string[] Inmunities { get; set; } = [];
        public string Icon { get; set; } = string.Empty;

    }
}