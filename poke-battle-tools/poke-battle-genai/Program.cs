using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using poke.battle.bridge;
using poke.battle.genai;
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
        p.WithOrigins("http://localhost:5070", "http://localhost:8080", "http://localhost:11434")
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
    var ollama = builder.Configuration["ollama-server"] ?? "http://localhost:11434/v1";
    kernel.AddOpenAIChatCompletion(modelId: "llama3", apiKey: "ollama", endpoint: new(ollama));
    return kernel.Build();
});

// Agent CRUD
builder.Services.AddSingleton<AgentTranslator>();
builder.Services.AddSingleton<AgentValidator>();
builder.Services.AddSingleton<AgentGenerator>();

// Agent Battle
builder.Services.AddScoped<IBattleContextFormatter, SimpleBattleContextFormatter>();
builder.Services.AddScoped<IBattleAgent, AgentBattle>();
builder.Services.AddScoped<IGeneratorAgent, AgentGenerator>();
builder.Services.AddSingleton<AgentNarrator>();
builder.Services.AddSingleton<AgentStrategy>();

// Agent Chat

builder.Services.AddSingleton<IAgentRouter, AgentRouter>();
builder.Services.AddScoped<IAgent,PingAgent>();
builder.Services.AddScoped<PingAgent>();

builder.WebHost.ConfigureKestrel(opt =>
{
    opt.ListenAnyIP(5071);
});

var app = builder.Build();
app.UseCors("AllowCors");
app.MapControllers();
app.Run();

