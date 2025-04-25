using System.IO.Compression;
using poke.battle.Models.Impl;

namespace poke.battle.core 
{
    public static class TypeEffectivenessResolver 
    {
        private static readonly Dictionary<string, TypeModel> _typesMap = new();

        public static void Initialize(IEnumerable<TypeModel> types) {
            foreach(var type in types)
            _typesMap[type.Name.ToLower()] = type;
        }

        public static double GetEffectiveness(string attack, string defender) {
            if(!_typesMap.TryGetValue(defender.ToLower(), out var defenderModel))
                return 1.0;
            
            if(defenderModel.Inmunities.Contains(attack, StringComparer.OrdinalIgnoreCase))
                return 0.0;
            if(defenderModel.Weakness.Contains(attack, StringComparer.OrdinalIgnoreCase))
                return 2.0;
            if(defenderModel.Resistences.Contains(attack, StringComparer.OrdinalIgnoreCase))
                return 0.5;
            return 1.0;
        } 

        public static double GetEffectivenessAgainst(string attack, string[] defender) {
            return defender.Select(t => GetEffectiveness(attack, t)).Aggregate(1.0, (acc, x) => acc * x);
        }

        internal static string GetText(double effectiveness)
        {
            return effectiveness switch{
                0.0 => "No afecta",
                < 1.0 => "No es muy eficaz",
                1.0 => "Es eficaz",
                > 1.0 => "¡Es muy eficaz!",
                _ => ""
            };
        }
    }
}