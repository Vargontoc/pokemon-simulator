using System.Text.Json;
using System.Text.Json.Serialization;
using poke.battle.bridge;
using poke.battle.bridge.Builders;
using poke.battle.bridge.Builders.Impl;
using poke.battle.bridge.Stores;
using poke.battle.bridge.Stores.Impl;
using poke.battle.core;
using poke.battle.infraestructure.repositories;
using poke.battle.infraestructure.repositories.impl;
using poke.battle.Models;
using poke.battle.Models.Impl;
using poke.battle.services;
using poke.battle.services.impl;
using poke_battle_api.dtos;
using poke_battle_api.mappers;
using poke_battle_api.mappers.impl;
using poke_battle_api.services;
using poke_battle_api.services.impl;
using poke_battle_api.utils;
using poke_battle_infraestructure.validators;
using poke_battle_infraestructure.validators.impl;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();
builder.Services.AddControllers()
.AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: false));
    });;

// Repositories
builder.Services.AddScoped<IAbilitiesRespository, AbilitiesRepository>();
builder.Services.AddScoped<ITypeRepository, TypesRepository>();
builder.Services.AddScoped<IMoveRepository, MovesRepository>();
builder.Services.AddScoped<IPokemonsRepository, PokemonsRepository>();

// Services
builder.Services.AddScoped<IAbilityService, AbilitiesService>();
builder.Services.AddScoped<ITypeService, TypesService>();
builder.Services.AddScoped<IMoveService, MovesService>();
builder.Services.AddScoped<IPokemonService, PokemonsService>();

// Mappers
builder.Services.AddScoped<IMapper<TypeDto, TypeModel>, TypeMapper>();
builder.Services.AddScoped<IMapper< MoveDto, MoveModel>, MoveMapper>();

builder.Services.AddCors(options => {
    options.AddPolicy("AllowCors", p => {
        p.WithOrigins("http://localhost:5071", "http://localhost:8080", "http://localhost:5173", "http://localhost:11434")
        
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

builder.Services.AddSingleton<IBattleStore, MemoryBattleStore>();
builder.Services.AddSingleton<IBattleContextBuilder, BattleContextBuilder>();
builder.Services.AddSingleton<IBattleContextFormatter, SimpleBattleContextFormatter>();

builder.Services.AddScoped<Validator<AbilityModel>, AbilityValidator>();
builder.Services.AddScoped<Validator<TypeModel>, TypeValidator>();

var baseUrl = builder.Configuration["poke-battle-ai"] ?? "http://localhost:5071";

builder.Services.AddSingleton<IBattleIAService, BattleIAService>();
builder.Services.AddHttpClient<IBattleIAService, BattleIAService>(client =>
{
    client.BaseAddress = new Uri(baseUrl!);
});

builder.Services.AddSingleton<IGeneratorAIService, GeneratorAIService>();
builder.Services.AddHttpClient<IGeneratorAIService, GeneratorAIService>(client =>
{
    client.BaseAddress = new Uri(baseUrl!);
});

builder.WebHost.ConfigureKestrel(opt =>
{
    opt.ListenAnyIP(5070);
});

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    
    var abs = services.GetRequiredService<IAbilityService>().FindAll(null!, null!).Results;
    var mvs = services.GetRequiredService<IMoveService>().FindAll(null!, null!).Results;
    var spc = services.GetRequiredService<IPokemonService>().FindAll(null!, null!).Results;
    var typ = services.GetRequiredService<ITypeService>().FindAll(null!, null!).Results;

    TypeEffectivenessResolver.Initialize(typ);
    CoreSettings.InitCore(abs, mvs, spc);
}
Calculator.InitializeTables();





app.UseCors("AllowCors");
app.MapControllers();
app.Run();