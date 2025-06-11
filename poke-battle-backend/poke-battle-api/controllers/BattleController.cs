using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using poke.battle.bridge;
using poke.battle.bridge.api;
using poke.battle.bridge.Api;
using poke.battle.bridge.Builders;
using poke.battle.bridge.Stores;
using poke.battle.core;
using poke.battle.Models;
using poke.battle.Models.helpers;
using poke.battle.Models.Impl;
using poke.battle.services;
using poke_battle_api.dtos;
using poke_battle_api.mappers.impl;
using System.Data.Common;
using System.Threading.Tasks;

namespace poke_battle_api.controllers
{
    [ApiController]
    [Route("api/battle")]
    public class BattleController(ILogger<BattleController> logger, ITypeService typeService, IPokemonService pokeService, IMoveService moveService, IBattleContextBuilder _builder, IBattleContextFormatter _formatter, IBattleStore _store, HttpClient _client) : ControllerBase
    {
        private readonly ILogger<BattleController> _logger = logger;
        private readonly IBattleContextBuilder _builder = _builder;
        private readonly IBattleContextFormatter _formatter = _formatter;
        private readonly IBattleStore _store = _store;
        private readonly HttpClient _http = _client;
        private readonly IPokemonService _pokeService = pokeService;
        private readonly IMoveService _moveService = moveService;
        private readonly ITypeService _typeService = typeService;


        [HttpPost("new")]
        public async Task<IActionResult> CreateBattle([FromBody] BattleBuildRequest request)
        {
            string idBattleGenerated = Guid.NewGuid().ToString();
            if(request.Battlers == null || request.Battlers.Length < 0 || request.Battlers.Length > 6)             {
                return BadRequest("Battlers is required.");
            }


            


            try
            {
                var speciesAvailables = _pokeService.FindAll(null!, null!).Results.Where(x => !string.IsNullOrEmpty(x.DisplayName));

                var player = CreateBattlers(request.Battlers);
                var iaSelection = new TeamSelectionRequest()
                {
                    PlayerSelection = player.Select(x => x.Specie.Name).ToList(),
                    Difficulty = "easy",
                    AvailableSpecies = speciesAvailables.Select(x => CreateSpecieInfo(x)).ToList()
                };


                var rng = new Random();
                var species = speciesAvailables.Select(x => x.Name).OrderBy(x => rng.Next()).Take(request.Battlers.Length).ToList();


                var decision = await _http.PostAsJsonAsync("http://localhost:5072/api/gen-ai/battle/select-team", iaSelection);
                var botResponse = await decision.Content.ReadFromJsonAsync<TeamSelectionResponse>();


                if(botResponse != null && botResponse.Team != null && botResponse.Team.Count != 0)
                {
                    _logger.LogInformation(botResponse.Reason);
                    species = botResponse.Team;
                }

                _store.Update(idBattleGenerated, new Battle(new BattleContext(
                    player: CreateBattlers(request.Battlers),
                    rival: [.. CreateMocks(species)]
                )));

            }catch(Exception e)
            {
                _logger.LogError("Could not start a battle: {0}", e);
                return BadRequest("Error");
            }


            return Ok(idBattleGenerated);

        }

        private static SpecieInfo CreateSpecieInfo(SpecieModel x)
        {
            return new SpecieInfo()
            {
                Specie = x.Name,
                Attk = x.StatsBase[1],
                Defense = x.StatsBase[2],
                AttkSpecial = x.StatsBase[3],
                DefenseSpecial = x.StatsBase[4],
                Speed = x.StatsBase[5],
                HP = x.StatsBase[0],
                Type1 = x.Types[0],
                Type2 = x.Types.Length == 2 ? x.Types[1] : string.Empty,
            };
        }

        private List<PBattler> CreateMocks(List<string> names)
        {
            List<PBattler> enemy = [];
            foreach (var name in names) 
            {
                enemy.Add(CreateMock(name));
            }

            enemy[0].IsActive = true;
            enemy[0].WasInBattle = true;
            return enemy;
        }

        private  List<PBattler> CreateBattlers(BuildBattlerDTO[] context)
        {
            List<PBattler> result = new List<PBattler>();
            foreach(var dto in context)
            {
  
                var specie = _pokeService.GetByName(dto.Specie);
                if (specie == null)
                    throw new Exception($"Specie {dto.Specie} not found");
                PBattler b = new(specie, dto.Level != 0 ? dto.Level : 100);

                var movepool = specie.Movepool
                    .Where(m => m.Item1 <= (dto.Level != 0 ? dto.Level : 100))
                    .Select(m => m.Item2)
                    .Distinct();
                var rng  = new Random();
                var moveNames = movepool.OrderBy(_ => rng.Next()).Take(movepool.ToList().Count > 4 ? 4: movepool.ToList().Count);

                if (dto.Moves != null && dto.Moves.Length > 0 && dto.Moves.Length <= 4)
                {
                    moveNames = _moveService.FindAll(null!, null!).Results.Where(x => dto.Moves.Contains(x.Name)).Select(x => x.Name);
                }
                var moves = moveNames
                                .Select(n => moveService.GetByName(n))
                                .Where(m => m != null)
                                .Select(m => new PBattleMove(m!))
                                .ToList();
                b.SetMoves([.. moves]);
                result.Add(b);
            }
            result[0].IsActive = true;
            result [0].WasInBattle = true;
            return result;
        }

        [HttpGet("{battleId}")]
        public IActionResult GetBattleInfo(string battleId)
        {
            var battle = _store.GetBattleById(battleId);
            if (battle == null) return NotFound($"Battle with ID {battleId} not found.");

            return Ok(CreateResponse(battle.Context.Player, battle.Context.Enemy));

        }

        private BattlerInfoResponse CreateResponse(List<PBattler> player, List<PBattler> enemy)
        {
            BattlerInfoResponse response = new();
            player.ForEach(battler => { response.Battlers.Add(CreateBattleInfo(battler)); } );
            enemy.ForEach(battler => { response.Battlers.Add(CreateBattleInfo(battler)); });

            return response;
        }


        private BattlerInfoDTO CreateBattleInfo(PBattler battler)
        {
            return new BattlerInfoDTO
            {
                Name = battler.Specie.DisplayName,
                Id = battler.Id,
                Hp = battler.CurrentHp,
                MaxHp = battler.MaxHP,
                Level = battler.Level,
                IsPlayer = battler.IsPlayer,
                IsActive = battler.IsActive,
                WasOnBattle = battler.WasInBattle,
                Stats = CreateStats(battler.Stats),
                Types = CreateTypes(battler.Types),
                Moves = battler.Moves.Select(m => new BattlerMoveInfo
                {
                    Name = m.Move.Name,
                    DisplayName = m.Move.DisplayName,
                    Type = CreateType(m.Move.Type),
                    Category = new
                    {
                        name = m.Move.MoveType,
                        icon = MoveMapper.GetIconFromCategory(m.Move.MoveType)
                    },
                    Effect = MoveEffect.Get(m.Move.MoveEffect).Description,

                    PP = m.CurrentPP,
                    Power = m.Move.Power != 0 ? m.Move.Power : null,
                    Accuracy = m.Move.Accuracy != 0 ? m.Move.Accuracy : null,
                    Priority = m.Move.Priority,

                    MaxPP = m.MaxPP
                }).ToList()

            };
        }

        private object CreateCategory(MoveType moveType)
        {
            throw new NotImplementedException();
        }

        private object CreateType(string type)
        {
            var t = _typeService.GetByName(type);
            if (!string.IsNullOrEmpty(t.Icon))
            {
                return new
                {
                    name = t.DisplayName,
                    icon = t.Icon,
                };
            }

            return new
            {
                name = t.DisplayName
            };
        }

        private object[] CreateTypes(TypeModel[] types)
        {
            List<object> result = new List<object>();
            foreach (TypeModel type in types) {
                var t = _typeService.GetByName(type.Name);
                if(!string.IsNullOrEmpty(t.Icon))
                {
                    result.Add(new
                    {
                        name = t.DisplayName,
                        icon = t.Icon,
                    });
                }else
                {
                    result.Add(new
                    {
                        name = t.DisplayName
                    });
                }

            }

            return [.. result];

        }

        private static object[] CreateStats(StatEntry[] stats)
        {
            var entries = stats.OrderBy(x => x.Stat.Index);
            List<object> result = new List<object>();
            foreach(var entry in entries)
            {
                result.Add(new {
                   abbr = entry.Stat.Abbr,
                   iv = entry.IV,
                   ev = entry.EV,
                   mod = entry.Modifier,
                   raw = entry.RawValue,
                   value = entry.BaseValue
                });
            }
            return [.. result];
        }

        [HttpGet("{battleId}/events")]
        public IActionResult GetLogBattle(string battleId)
        {
            var battle = _store.GetBattleById(battleId);
            if (battle == null) return NotFound($"Battle with ID {battleId} not found.");
            return Ok(battle.Events);
        }


        private PBattler CreateMock(string v)
        {
            SpecieModel specie = pokeService.GetByName(v.ToLower());
            var movepool = specie.Movepool
             .Where(m => m.Item1 <= 50)
             .Select(m => m.Item2)
             .Distinct();
            var rng = new Random();
            var moveNames = movepool.OrderBy(_ => rng.Next()).Take(movepool.ToList().Count > 4 ? 4 : movepool.ToList().Count);
            var moves = moveNames
                .Select(n => moveService.GetByName(n))
                .Where(m => m != null)
                .Select(m => new PBattleMove(m!))
                .ToList();
            var result = new PBattler(specie, 100, false);
            result.SetMoves([.. moves]);
            _logger.LogInformation("Creating mock battler for {Specie} with moves: {Moves}", specie.Name, string.Join(", ", moves.Select(x => x.Move.Name)));
            return result;
        }

        [HttpPost("{battleId}/turn")]
        public async Task<IActionResult> PlayTurn(string battleId, [FromBody] BattlePlayerRequest playerAction)
        {
            var battle = _store.GetBattleById(battleId);
            if(battle == null) return NotFound($"Battle with ID {battleId} not found.");

            if (playerAction.Action == BattlePlayerRequest.BattleTypeRequest.Switch)
                battle.QueueSwitchAction(playerAction.Value, battle.Context.Player.First(x => x.IsActive));

            if (playerAction.Action == BattlePlayerRequest.BattleTypeRequest.Surronder)
                return Ok($"Player fainted!");

            if (playerAction.Action == BattlePlayerRequest.BattleTypeRequest.Attack)
            {
                battle.QueueAttackAction(playerAction.Value, battle.Context.Player.First(x => x.IsActive));
            }

           
            var prompt = _formatter.FormatAsText(battle);

            var aiRequest = new BattleDecisionRequest()
            {
                BotName = "AI_Bot",
                Prompt = prompt,
            };

            var decision = await _http.PostAsJsonAsync("http://localhost:5072/api/gen-ai/battle/decide", aiRequest);
            var botResponse = await decision.Content.ReadFromJsonAsync<BattleDecisionResponse>();

            if(botResponse == null) return StatusCode(500, "Failed to get a response from the AI bot.");
            _logger.LogInformation("AI Bot Response: {Action} - {Value} - {Rationale}", botResponse.Action, botResponse.Value, botResponse.Rationale);
            if (botResponse.Action.Equals("move"))
            {
                battle.QueueAttackAction(botResponse.Value, battle.Context.Enemy.First(x => x.IsActive));
            }

            if(botResponse.Action.Equals("switch"))
            {
                battle.QueueAttackAction(botResponse.Value, battle.Context.Enemy.First(x => x.IsActive));
            }

            battle.Execute();

            return Ok(new { events = battle.Events});
        }
  
    }
}
