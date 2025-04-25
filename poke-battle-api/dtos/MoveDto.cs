namespace poke_battle_api.dtos
{
    public class MoveDto
    {
        public int Id { get; set; }
        public string InternalName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TypeDto Type { get; set; }
        public int Power { get; set; }
        public int Accuracy { get; set; }
        public int Priority { get; set; }
        public string Category { get; set; } = string.Empty;
        public string IconCategory {  get; set; } = string.Empty;

    }
}
