using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace poke.battle.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class MoveModel : IModel
    {
        /// <summary>
        /// Obtiene o establece el nombre interno del movimiento
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set;} = string.Empty;
        /// <summary>
        /// Obtiene o establece el identificador del movimiento
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Obtiene o establece el nombre del movimiento
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DisplayName { get; set; } = string.Empty;
        /// <summary>
        /// Obtiene o establece el nombre abreviado del movimiento
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Abbr { get; set; } = string.Empty; 
        /// <summary>
        /// Obtiene o establece la descripcion del movimiento
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description {get; set; } = string.Empty;
        /// <summary>
        /// Obtiene o establece el tipo del movimiento
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; } = string.Empty;
        /// <summary>
        /// Obtiene o establece la potencia del movimiento
        /// </summary>
        public int Power {get; set; }
        /// <summary>
        /// Obtiene o establece la precisión del movimiento
        /// </summary>
        public int Accuracy { get; set; }
        /// <summary>
        /// Obtiene o establece la prioridad del movimiento
        /// </summary>
        public int Priority { get; set; }
        /// <summary>
        /// Obtiene o establece el codigo de efecto del movimiento
        /// </summary>
        public int MoveEffect { get; set; } = 0x000; 
        public int? SecondaryEffect { get; set; } = 0x000; 
        /// <summary>
        /// Obtiene los PP base del movimiento
        /// </summary>
        public int PP { get; set; } 
        [JsonConverter(typeof(StringEnumConverter))]
        public MoveType MoveType { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public TargetType Target { get; set; }
    }

    public enum MoveType 
    {
        Physical, Special, Status
    }

    public enum TargetType 
    {
        Self, Enemy, AllEnemies, AllAllies, All, Random
    }
}