using System.Text;

namespace poke.battle.bridge.Api
{
    public class TeamSelectionRequest
    {
        public List<string> PlayerSelection { get; set; } = [];
        public string Difficulty { get; set; } = "normal";
        public List<SpecieInfo> AvailableSpecies { get; set; } = [];
        public string ToPrompt()
        {
            var sb = new StringBuilder();
            sb.AppendLine("\nEspecies disponibles: ");
            foreach ( var item in AvailableSpecies)
            {
                sb.AppendLine(item.Specie);
            }

            sb.AppendLine();
            sb.AppendLine($"Dificultad: {Difficulty}");
            sb.AppendLine($"Selecciona exactamente {PlayerSelection.Count} Pokémon de la lista anterior de manera aleatoria. Devuelve un JSON valido.");



            return sb.ToString();
        }
    }
}
