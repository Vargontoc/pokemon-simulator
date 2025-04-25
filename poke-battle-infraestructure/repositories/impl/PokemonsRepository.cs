using System.Data;
using System.Runtime.InteropServices.Marshalling;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using poke.battle.infraestructure.filters;
using poke.battle.Models.Impl;

namespace poke.battle.infraestructure.repositories.impl
{
    public class PokemonsRepository : GenericRepository<SpecieModel, PokemonsFilter>, IPokemonsRepository
    {
        private HttpClient _client;
        public PokemonsRepository(HttpClient client):base()
        {
            STORED_FILE = "pokemons.json";
            _client = client;
            this._client.BaseAddress = new(URI_POKE_API);

            Initialize();
        }

        public override async void Initialize()
        {
            if(!File.Exists(GetFullPath())) 
            {
                var res = await _client.GetAsync("pokemon?limit=1302&offset=0");
                if(!res.IsSuccessStatusCode)
                {
                    throw new RepositoryException("Could not initialize repository");
                }   

                var json = await res.Content.ReadAsStringAsync();
                dynamic? data = JsonConvert.DeserializeObject(json);

                if(data == null) return;

                List<SpecieModel> models = new();
                var tasks = new List<Task<SpecieModel>>();

                using var semaphore = new SemaphoreSlim(10);
                int count = 0;
                foreach(var entry in data.results) 
                {
                    await semaphore.WaitAsync();
                    tasks.Add(Loading(entry, semaphore, ++count));
                }

                var results = await Task.WhenAll(tasks);
                models = results.Where(m => m != null).Cast<SpecieModel>().ToList();
                Save(models);
                
            }
        }

        private async Task<SpecieModel?> Loading(dynamic entry, SemaphoreSlim semaphore, int count) 
        {
            try {
                string name = entry.name;
                string url = entry.url;

                var pokemonRes = await _client.GetAsync(url);
                if(!pokemonRes.IsSuccessStatusCode) return null;

                var pokemonJson = await pokemonRes.Content.ReadAsStringAsync();
                dynamic pokemonData = JsonConvert.DeserializeObject(pokemonJson)!;

                string specieUrl = pokemonData.species.url;
                var speciesRes = await _client.GetAsync(specieUrl);

                if(!speciesRes.IsSuccessStatusCode) return null;

                var specieJson = await speciesRes.Content.ReadAsStringAsync();
                dynamic specieData = JsonConvert.DeserializeObject(specieJson)!;


                return await Map(pokemonData, specieData, count);

            }catch(Exception ex)
            {
                Console.WriteLine($"[ERROR] {entry.name}: {ex.Message}");
                return null;
            }finally { semaphore.Release(); }    
        }
        private  async Task<SpecieModel> Map(dynamic pokeData, dynamic specieData, int count)
        {

            // Types
            string[] types = ((JArray)pokeData.types)
                                .OrderBy(t => (int)t["slot"]!)
                                .Select(t => (string)t["type"]!["name"]!).ToArray();
            
            // Abilities
            var abilitiesJson = ((JArray)pokeData.abilities);
            string[] abilities = abilitiesJson.Where(a => a["is_hidden"]!.ToObject<bool>() == false)
                                                .Select(a => (string)a["ability"]!["name"]!).ToArray();
            string hiddenAbility = abilitiesJson.Where(a => a["is_hidden"]!.ToObject<bool>() == true)
                                                .Select(a => (string)a["ability"]!["name"]!).FirstOrDefault() ?? string.Empty;

            // Stats
           int[] stats = [
                ((JArray)pokeData.stats).Where(s => ((string)s["stat"]!["name"]!).Equals("hp")).Select(s => (int)s["base_stat"]!).FirstOrDefault(),
                ((JArray)pokeData.stats).Where(s => ((string)s["stat"]!["name"]!).Equals("attack")).Select(s => (int)s["base_stat"]!).FirstOrDefault(),
                ((JArray)pokeData.stats).Where(s => ((string)s["stat"]!["name"]!).Equals("defense")).Select(s => (int)s["base_stat"]!).FirstOrDefault(),
                ((JArray)pokeData.stats).Where(s => ((string)s["stat"]!["name"]!).Equals("special-attack")).Select(s => (int)s["base_stat"]!).FirstOrDefault(),
                ((JArray)pokeData.stats).Where(s => ((string)s["stat"]!["name"]!).Equals("special-defense")).Select(s => (int)s["base_stat"]!).FirstOrDefault(),
                ((JArray)pokeData.stats).Where(s => ((string)s["stat"]!["name"]!).Equals("speed")).Select(s => (int)s["base_stat"]!).FirstOrDefault()
           ];

            // Efforts
           int[] efforts = [
                            ((JArray)pokeData.stats).Where(s => ((string)s["stat"]!["name"]!).Equals("hp")).Select(s => (int)s["effort"]!).FirstOrDefault(),
                ((JArray)pokeData.stats).Where(s => ((string)s["stat"]!["name"]!).Equals("attack")).Select(s => (int)s["effort"]!).FirstOrDefault(),
                ((JArray)pokeData.stats).Where(s => ((string)s["stat"]!["name"]!).Equals("defense")).Select(s => (int)s["effort"]!).FirstOrDefault(),
                ((JArray)pokeData.stats).Where(s => ((string)s["stat"]!["name"]!).Equals("special-attack")).Select(s => (int)s["effort"]!).FirstOrDefault(),
                ((JArray)pokeData.stats).Where(s => ((string)s["stat"]!["name"]!).Equals("special-defense")).Select(s => (int)s["effort"]!).FirstOrDefault(),
                ((JArray)pokeData.stats).Where(s => ((string)s["stat"]!["name"]!).Equals("speed")).Select(s => (int)s["effort"]!).FirstOrDefault()
           ];
            
            // Moves
            var movesJson = (JArray)pokeData.moves;
            List<Tuple<int, string>> learned = new();
            List<string> eggMoves = new();
            List<string> tmMoves = new();
            List<string> tutorMoves = new();

            foreach(var move in movesJson) 
            {
                string moveName = (string)move["move"]!["name"]!;
                foreach(var detail in move["version_group_details"]!) {
                    string method = (string)detail["move_learn_method"]?["name"]!;
                    switch(method) {
                        case "level-up" :
                            int level = (int)detail["level_learned_at"]!;
                            if(level != 0 && !learned.Any(x => x.Item2 == moveName))
                                learned.Add(Tuple.Create(level, moveName));
                            break;
                        case "egg":
                            if(!eggMoves.Contains(moveName))
                                eggMoves.Add(moveName);
                            break;
                        case "machine":
                            if(!tmMoves.Contains(moveName))
                                tmMoves.Add(moveName);
                            break;
                        case "tutor":
                            if(!tutorMoves.Contains(moveName))
                            tutorMoves.Add(moveName);
                            break;
                    }
                }
            }

            // Egg Group
            string[] eggGroups = ((JArray)specieData.egg_groups).Select(g => (string)g["name"]!).ToArray();
            
            
   
            // Held Items
            List<WildItem> items = new();
            foreach(var held in pokeData.held_items) 
            {
                var item = (string)held.item.name;
                var  rarity = (int)held.version_details[0].rarity;

                items.Add(new(){
                    Item = item,
                    Rarity = rarity
                });
            }            
            // Evolutions
            List<EvolutionChain> evolutions = [];
            var evoUrl = (string)specieData.evolution_chain.url;
            var evoRes = await _client.GetAsync(evoUrl);
            string name = pokeData.name;
            if(evoRes.IsSuccessStatusCode)
            {
                var evoJson = await evoRes.Content.ReadAsStringAsync();
                dynamic evoData = JsonConvert.DeserializeObject(evoJson)!;
                dynamic chain = evoData.chain;     

                while(chain != null && chain.species.name != name && chain.evolves_to.Count > 0) 
                {
                    chain = chain.evolves_to[0];
                }

                if(chain.species.name == pokeData.name && chain.evolves_to.Count > 0) {

                    foreach(var evo in chain.evolves_to)
                    {
                        string target = evo.species.name;
                        var evolution = new EvolutionChain
                        {
                            Specie = target
                        };
                        

                        foreach(var detail in evo.evolution_details) 
                        {
                            evolution.Trigger = EvolutionTrigger.Get((string)detail.trigger.name);
                            var requirements = new List<EvolutionRequirement>();

                            void AddReq(EvolutionCondition cond, dynamic? val) {
                                if(requirements.Any(x => x.Condition.Code.Equals(cond.Code)))
                                    return;

                                    if (val == null || (val is string str && string.IsNullOrWhiteSpace(str)))
                                         return;

                                // Si es un objeto con propiedad "name", como un objeto dinámico
                                string valueString = val.GetType().GetProperty("name") != null
                                    ? (string)val.name
                                    : val.ToString();



                                    requirements.Add(new() {
                                        Condition = cond,
                                        Value = valueString
                                    });
                                
                            }

                            

                            AddReq(EvolutionCondition.Item, detail.item);
                            AddReq(EvolutionCondition.HeldItem, detail.held_item);
                            AddReq(EvolutionCondition.KnownMove, detail.known_move);
                            AddReq(EvolutionCondition.KnownMoveType, detail.known_move_type);
                            AddReq(EvolutionCondition.Level, detail.min_level);
                            AddReq(EvolutionCondition.Happiness, detail.min_happiness);
                            AddReq(EvolutionCondition.TimeOfDay, detail.time_of_day);
                            AddReq(EvolutionCondition.RelativeStats, detail.relative_physical_stats);
                            
                            evolution.Requirements = requirements.ToArray();
                        }
                         evolutions.Add(evolution);
                    }

                }

            }

            return new SpecieModel() 
            {
                Id = count,
                Name = pokeData.name,
                Types = types,
                Abilities = abilities,
                HiddenAbility = hiddenAbility,
                StatsBase = stats,
                Efforts = efforts,
                Movepool = [.. learned.OrderBy(l => l.Item1)],
                Happiness = (int)(specieData.base_happiness ?? 70),
                EggMoves = eggMoves.ToArray(),
                MtMoves = [.. tmMoves],
                TutorMoves = tutorMoves.ToArray(),
                Habitat = Habitat.Get((string)specieData.habitat?.name! ?? string.Empty),
                Color = Color.Get((string)specieData.color?.name! ?? string.Empty),
                EggGroups = [.. eggGroups.Select(x => EggGroup.Get(x))],
                Height = (double)pokeData.height,
                Weight = (double)pokeData.weight,
                Growth = Growth.Get((string)specieData.growth_rate?.name! ?? string.Empty),
                CaptureRate = specieData.capture_rate ?? 0,
                Exp = pokeData.base_experience ?? 0,
                Evolutions = evolutions.ToArray(),
                HeldItems = items.ToArray()

            };
        }

        protected override void CreatePredicates(PokemonsFilter filter)
        {
            // Empty method
        }

        public void SaveAll(List<SpecieModel> species)
        {
            Save(species);
        }
    }
}