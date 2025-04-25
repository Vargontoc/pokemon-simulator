using Newtonsoft.Json;
using poke.battle.infraestructure.filters;
using poke.battle.Models;

namespace poke.battle.infraestructure.repositories.impl
{
    public class MovesRepository : GenericRepository<MoveModel, MovesFilter>, IMoveRepository
    {
        private HttpClient _client;
        public MovesRepository(HttpClient client):base()
        {
            STORED_FILE = "moves.json";
            _client = client;
            this._client.BaseAddress = new(URI_POKE_API);

            Initialize();
        }

        public override async void Initialize()
        {
            if(!File.Exists(GetFullPath())) 
            {
                var res = await _client.GetAsync("move?limit=1000&offset=0");
                if(!res.IsSuccessStatusCode)
                {
                    throw new RepositoryException("Could not initialize repository");
                }   

                var json = await res.Content.ReadAsStringAsync();
                dynamic? data = JsonConvert.DeserializeObject(json);
                if(data != null)
                {
                    List<MoveModel> models = new();
                    int count = 1;
                    foreach(var d in data.results)
                    {
                        string url = d.url;
                        var details = await _client.GetAsync(url);
                        if(!details.IsSuccessStatusCode){
                            throw new RepositoryException($"Error retreiving type details for {d.name}");
                        }

                        var detailJson = details.Content.ReadAsStringAsync();
                        dynamic detailData  = JsonConvert.DeserializeObject(detailJson.Result)!;                        
                    
                        models.Add(Map(detailData, count++));
                    
                    }


                    Save(models);
                }
            }
        }

        private MoveModel Map(dynamic data, int count)
        {
            return new MoveModel 
            {
                Id = count,
                Name = data.name,
                Type = data.type.name,
                Power = data.power != null ? (int)data.power : 0,
                Accuracy = data.accuracy != null ? (int)data.accuracy : 0,
                Priority = (int)data.priority,
                MoveType = MapMoveType((string)data.damage_class.name),
                Target = MapTarget((string)data.target.name),
                PP = data.pp != null ? (int)data.pp : 0
            };
        }

        private MoveType MapMoveType(string v) 
        {
            return v switch {
                "physical" => MoveType.Physical,
                "special" => MoveType.Special,
                "status" => MoveType.Status,
                _ => MoveType.Physical
            };
        }
        private TargetType MapTarget(string v)
        {
            return v switch {
                "selected-pokemon" => TargetType.Enemy,
                "all-opponents" => TargetType.AllEnemies,
                "user" => TargetType.Self,
                "entire-field" => TargetType.All,
                _ => TargetType.Enemy
            };
        }

        protected override void CreatePredicates(MovesFilter filter)
        {
            // Empty Method
        }

        public void SaveAll(List<MoveModel> models)
        {
            Save(models);
        }
    }
}