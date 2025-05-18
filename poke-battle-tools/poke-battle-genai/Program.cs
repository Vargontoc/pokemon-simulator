using Microsoft.SemanticKernel;
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



var app = builder.Build();
app.UseCors("AllowCors");
app.MapControllers();
app.Run();
