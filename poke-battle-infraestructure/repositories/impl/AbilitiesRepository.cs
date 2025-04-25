using System.Net.Http.Headers;
using Newtonsoft.Json;
using poke.battle.infraestructure.filters.impl;
using poke.battle.Models.Impl;

namespace poke.battle.infraestructure.repositories.impl
{
    public class AbilitiesRepository : GenericRepository<AbilityModel, AbilitiesFilter>, IAbilitiesRespository
    {
        private HttpClient _client;
        public AbilitiesRepository(HttpClient client):base()
        {
            STORED_FILE = "abilities.json";
            _client = client;
            this._client.BaseAddress = new(URI_POKE_API);

            Initialize();
        }

        public override async void Initialize()
        {
            if(!File.Exists(GetFullPath())) 
            {
                var res = await _client.GetAsync("ability?limit=367&offset=0");
                if(!res.IsSuccessStatusCode)
                {
                    throw new RepositoryException("Could not initialize repository");
                }   

                var json = await res.Content.ReadAsStringAsync();
                dynamic? data = JsonConvert.DeserializeObject(json);
                if(data  != null)
                {
                    List<AbilityModel> models = new();
                    int count = 1;
                    foreach(var p in data.results) 
                    {   
                        models.Add(new() { Id = count++, Name = p.name });
                    }

                    Save(models);
                }
            }
        }

        public void SaveAll(List<AbilityModel> abilities)
        {
            Save(abilities);
        }

        protected override void CreatePredicates(AbilitiesFilter filter)
        {
            // Empty method
        }
    }
}