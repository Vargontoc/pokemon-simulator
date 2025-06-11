using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace poke.battle.bridge
{
    public class SpecieInfo
    {
        public string Specie {  get; set; } = string.Empty;
        public int HP { get; set; }
        public int Attk { get; set; }
        public int Defense { get; set; }
        public int AttkSpecial { get; set; }
        public int DefenseSpecial { get; set; }
        public int Speed { get; set; }
        public string Type1 { get; set; } = string.Empty;
        public string Type2 { get; set; } = string.Empty;

        public string ToString()
        {
            return $"- {Specie} (HP: {HP}, Atk: {Attk}, Def: {Defense}, SpAtk: {AttkSpecial}, SpDef: {DefenseSpecial}, Spe: {Speed}, Tipo: {Type1}{(string.IsNullOrEmpty(Type2) ? "" : "/" + Type2)})";
        }
    }
}
