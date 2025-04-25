using poke.battle.Models.Impl;

namespace poke.battle.infraestructure.filters
{
    public class TypesFilter : IFilter<TypeModel>
    {
        public string Search { get; set; } = string.Empty;
        public string WeakTo {  get; set; } = string.Empty;
        public string ResistenceTo { get; set; } = string.Empty;
        public string InmunityTo { get; set; } = string.Empty;
    }
}