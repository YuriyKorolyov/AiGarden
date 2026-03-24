using AiGarden.BuildingBlocks.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAiGardenAuth(builder.Configuration);
builder.Services.AddAiGardenTelemetry(builder.Configuration, "AiGarden.YarpGateway");
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapReverseProxy();
app.MapGet("/", () => Results.Ok(new
{
    Service = "AiGarden Gateway",
    TimeUtc = DateTimeOffset.UtcNow
}));

app.Run();
