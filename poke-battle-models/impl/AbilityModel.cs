using Newtonsoft.Json;

namespace poke.battle.Models.Impl
{
    public class AbilityModel : IModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; } = string.Empty;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DisplayName { get; set; } = string.Empty;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Abbr { get; set; } = string.Empty;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description {get; set;} = string.Empty;
        public int Id { get; set; } = 0;
    }
} 