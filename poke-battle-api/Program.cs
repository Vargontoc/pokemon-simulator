using System.Text.Json;
using System.Text.Json.Serialization;
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

var builder = WebApplication.CreateBuilder(args);
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
    options.AddPolicy("AllowVueDev", p => {
        p.WithOrigins("http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
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

    app.UseCors("AllowVueDev");
app.MapControllers();
app.Run();