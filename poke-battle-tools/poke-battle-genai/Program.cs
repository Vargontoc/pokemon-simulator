using Microsoft.SemanticKernel;
using poke.battle.bridge;
using poke.battle.genai.Models;
using poke.battle.genai.Models.Agents;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: false));
});
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowCors", p =>
    {
        p.WithOrigins("http://localhost:5173", "http://localhost:11424", "http://localhost:80", "http://localhost:8080")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();
    });
});

// IA Container 
builder.Services.AddSingleton<ChatMemory>();
builder.Services.AddSingleton<Kernel>(sp =>
{
    var kernel = Kernel.CreateBuilder();
    kernel.AddOpenAIChatCompletion(modelId: "llama3", apiKey: "ollama", endpoint: new("http://localhost:11434/v1"));
    return kernel.Build();
});

// Agent CRUD
builder.Services.AddSingleton<AgentTranslator>();
builder.Services.AddSingleton<AgentValidator>();
builder.Services.AddSingleton<AgentGenerator>();

// Agent Battle
builder.Services.AddScoped<IBattleContextFormatter, SimpleBattleContextFormatter>();
builder.Services.AddScoped<IBattleAgent, AgentBattle>();
builder.Services.AddSingleton<AgentNarrator>();
builder.Services.AddSingleton<AgentStrategy>();

// Agent Chat

builder.Services.AddSingleton<IAgentRouter, AgentRouter>();

builder.WebHost.ConfigureKestrel(opt =>
{
    opt.ListenAnyIP(5072);
    opt.ListenAnyIP(5073, lo => lo.UseHttps());
});

var app = builder.Build();
app.UseCors("AllowCors");
app.MapControllers();
app.Run();
