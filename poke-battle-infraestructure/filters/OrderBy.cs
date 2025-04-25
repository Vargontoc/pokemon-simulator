namespace poke.battle.infraestructure.filters
{ 
    public class OrderBy 
    {
        public string Property { get; set; } = string.Empty;
        public OrderDirection Direction { get; set; } = OrderDirection.asc;
    }
}