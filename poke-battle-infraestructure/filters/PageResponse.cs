namespace poke.battle.infraestructure.filters
{
    public class PageResponse<T> 
    {
        public int Page { get; set; } = 1;
        public int PageSize {get; set; } = 25;
        public int Total { get; set; } = 1;
        public List<T> Results { get; set;} = [];  
    }
}