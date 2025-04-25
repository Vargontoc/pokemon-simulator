using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using poke.battle.infraestructure.filters;
using poke.battle.Models.Impl;

namespace poke.battle.infraestructure.repositories.impl
{
    public class TypesRepository : GenericRepository<TypeModel, TypesFilter>, ITypeRepository 
    {
        private HttpClient _client; 
        public TypesRepository(HttpClient client): base() {

            STORED_FILE = "types.json";
            _client = client;
            _client.BaseAddress = new(URI_POKE_API);

            Initialize();
        }

        public override async void Initialize()
        {
            if(!File.Exists(GetFullPath())) 
            {
                var res = await _client.GetAsync("type?limit=25&offset=0");
                if(!res.IsSuccessStatusCode)
                {
                    throw new RepositoryException("Could not initialize repository");
                }   

                var json = await res.Content.ReadAsStringAsync();
                dynamic? data = JsonConvert.DeserializeObject(json);
                if(data  != null)
                {
                    List<TypeModel> models = new();
                    int count = 1;
                    foreach(var p in data.results) 
                    {   
                        string name = p.name;
                        string url = p.url;

                        var details = await _client.GetAsync(url);
                        if(!details.IsSuccessStatusCode){
                            throw new RepositoryException($"Error retreiving type details for {name}");
                        }

                        var detailJson = details.Content.ReadAsStringAsync();
                        dynamic detailData  = JsonConvert.DeserializeObject(detailJson.Result)!;

                        var relations = detailData!.damage_relations;
                        string[] resistances = ((JArray)relations.half_damage_from).Select(x => (string)x["name"]!).ToArray();
                        string[] weaknesses = ((JArray)relations.double_damage_from).Select(x => (string)x["name"]!).ToArray();
                        string[] inmunities = ((JArray)relations.no_damage_from).Select(x => (string)x["name"]!).ToArray();

                        models.Add(new TypeModel() {
                            Id = count++,
                            Name = name,
                            Resistences = resistances,
                            Weakness = weaknesses,
                            Inmunities = inmunities
                        });
                    }

                    Save(models);
                }
            }
        }

        protected override void CreatePredicates(TypesFilter filter)
        {
            if(!string.IsNullOrEmpty(filter.WeakTo))
            {
                predicates.Add(x => x.Weakness.Contains(filter.WeakTo));
            }
            if (!string.IsNullOrEmpty(filter.ResistenceTo))
            {
                predicates.Add(x => x.Resistences.Contains(filter.ResistenceTo));
            }
            if (!string.IsNullOrEmpty(filter.InmunityTo))
            {
                predicates.Add(x => x.Inmunities.Contains(filter.InmunityTo));
            }
        }
    } 
}