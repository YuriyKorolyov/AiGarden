using AiGarden.AiService.Infrastructure;
using AiGarden.AiService.Infrastructure.Persistence;
using AiGarden.AiService.UseCases.Commands;
using AiGarden.AiService.UseCases.Queries;
using AiGarden.BuildingBlocks.Abstractions;
using AiGarden.BuildingBlocks.Web;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAiGardenAuth(builder.Configuration);
builder.Services.AddAiGardenTelemetry(builder.Configuration, "AiGarden.AiService");
builder.Services.AddAiInfrastructure(builder.Configuration);
builder.Services.AddScoped<ICommandHandler<StartPlantAnalysisCommand, AiGarden.Contracts.Responses.AnalysisAcceptedResponse>, StartPlantAnalysisCommandHandler>();
builder.Services.AddScoped<IQueryHandler<GetPlantAnalysisQuery, AiGarden.Contracts.Responses.AnalysisResultResponse?>, GetPlantAnalysisQueryHandler>();
builder.Services.AddValidatorsFromAssemblyContaining<StartPlantAnalysisCommandValidator>();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AiDbContext>();
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
