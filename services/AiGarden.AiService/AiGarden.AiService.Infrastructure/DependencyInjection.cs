using AiGarden.AiService.Core.Repositories;
using AiGarden.AiService.Core.Services;
using AiGarden.AiService.Infrastructure.Options;
using AiGarden.AiService.Infrastructure.Persistence;
using AiGarden.AiService.Infrastructure.Processing;
using AiGarden.AiService.Infrastructure.Providers;
using AiGarden.AiService.Infrastructure.Streaming;
using AiGarden.AiService.UseCases.Abstractions;
using AiGarden.BuildingBlocks.Extensions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AiGarden.AiService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddAiInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<NvidiaOptions>(configuration.GetSection(NvidiaOptions.SectionName));
        services.Configure<OllamaOptions>(configuration.GetSection(OllamaOptions.SectionName));

        services.AddDbContext<AiDbContext>(options =>
            options.UseNpgsql(configuration.GetRequiredConnectionString("AiDb")));

        services.AddScoped<IPlantAnalysisRepository, PlantAnalysisRepository>();

        services.AddSingleton<PlantAnalysisBackgroundQueue>();
        services.AddSingleton<IPlantAnalysisQueue>(sp => sp.GetRequiredService<PlantAnalysisBackgroundQueue>());
        services.AddSingleton<IAnalysisEventBroker, InMemoryAnalysisEventBroker>();
        services.AddHostedService<PlantAnalysisWorker>();

        services.AddHttpClient("nvidia");
        services.AddHttpClient("photos");
        services.AddHttpClient("ollama", (sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<OllamaOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl);
        });

        services.AddScoped<IPlantDiagnosisProvider, NvidiaPlantDiagnosisProvider>();
        services.AddScoped<IPlantDiagnosisProvider, OllamaPlantDiagnosisProvider>();

        services.AddMassTransit(configurator =>
        {
            configurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(configuration["RabbitMq:Host"] ?? "amqp://localhost:5672"), host =>
                {
                    host.Username(configuration["RabbitMq:Username"] ?? "guest");
                    host.Password(configuration["RabbitMq:Password"] ?? "guest");
                });
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
