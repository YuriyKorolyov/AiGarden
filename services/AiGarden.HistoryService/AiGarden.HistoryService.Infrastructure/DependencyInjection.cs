using AiGarden.BuildingBlocks.Extensions;
using AiGarden.HistoryService.Core.Repositories;
using AiGarden.HistoryService.Infrastructure.Consumers;
using AiGarden.HistoryService.Infrastructure.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AiGarden.HistoryService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddHistoryInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<HistoryDbContext>(options =>
            options.UseNpgsql(configuration.GetRequiredConnectionString("HistoryDb")));

        services.AddScoped<IAnalysisHistoryRepository, AnalysisHistoryRepository>();

        services.AddMassTransit(configurator =>
        {
            configurator.AddConsumer<PlantAnalysisCompletedConsumer>();
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
