using AiGarden.BuildingBlocks.Extensions;
using AiGarden.UserService.Core.Repositories;
using AiGarden.UserService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AiGarden.UserService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddUserInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UserDbContext>(options =>
            options.UseNpgsql(configuration.GetRequiredConnectionString("UserDb")));

        services.AddScoped<IAppUserRepository, AppUserRepository>();
        return services;
    }
}
