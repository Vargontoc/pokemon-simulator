namespace poke.battle.infraestructure.filters
{ 
    public class PageRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public OrderBy? OrderBy { get; set; }
    }
}