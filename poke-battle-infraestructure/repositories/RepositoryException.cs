namespace poke.battle.infraestructure.repositories
{
    public class RepositoryException : Exception
    {
        public RepositoryException(string msg): base(msg) {}
        public RepositoryException(string msg, Exception e) : base(msg, e) {}
    }    
}