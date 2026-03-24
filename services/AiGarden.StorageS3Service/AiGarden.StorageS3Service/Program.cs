using AiGarden.BuildingBlocks.Abstractions;
using AiGarden.BuildingBlocks.Web;
using AiGarden.StorageS3Service.Infrastructure;
using AiGarden.StorageS3Service.Infrastructure.Persistence;
using AiGarden.StorageS3Service.UseCases.Commands;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAiGardenAuth(builder.Configuration);
builder.Services.AddAiGardenTelemetry(builder.Configuration, "AiGarden.StorageS3Service");
builder.Services.AddStorageInfrastructure(builder.Configuration);
builder.Services.AddScoped<ICommandHandler<UploadPlantPhotoCommand, AiGarden.Contracts.Responses.FileUploadResponse>, UploadPlantPhotoCommandHandler>();
builder.Services.AddValidatorsFromAssemblyContaining<UploadPlantPhotoCommandValidator>();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<StorageDbContext>();
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
