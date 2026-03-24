using AiGarden.BuildingBlocks.Abstractions;
using AiGarden.BuildingBlocks.Web;
using AiGarden.HistoryService.Infrastructure;
using AiGarden.HistoryService.Infrastructure.Persistence;
using AiGarden.HistoryService.UseCases.Queries;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAiGardenAuth(builder.Configuration);
builder.Services.AddAiGardenTelemetry(builder.Configuration, "AiGarden.HistoryService");
builder.Services.AddHistoryInfrastructure(builder.Configuration);
builder.Services.AddScoped<IQueryHandler<GetUserHistoryQuery, IReadOnlyCollection<AiGarden.Contracts.Responses.AnalysisHistoryItemResponse>>, GetUserHistoryQueryHandler>();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<HistoryDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
