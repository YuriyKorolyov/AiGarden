using AiGarden.BuildingBlocks.Abstractions;
using AiGarden.BuildingBlocks.Web;
using AiGarden.UserService.Infrastructure;
using AiGarden.UserService.Infrastructure.Persistence;
using AiGarden.UserService.UseCases.Commands;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAiGardenAuth(builder.Configuration);
builder.Services.AddAiGardenTelemetry(builder.Configuration, "AiGarden.UserService");
builder.Services.AddUserInfrastructure(builder.Configuration);
builder.Services.AddScoped<ICommandHandler<SyncCurrentUserCommand, AiGarden.Contracts.Responses.UserProfileResponse>, SyncCurrentUserCommandHandler>();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
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
