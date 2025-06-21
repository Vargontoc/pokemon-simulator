
namespace poke_battle_api.services.impl
{
    public abstract class AbstractIAService : IAIService
    {
        public abstract HttpClient Client { get; }
    }
}
