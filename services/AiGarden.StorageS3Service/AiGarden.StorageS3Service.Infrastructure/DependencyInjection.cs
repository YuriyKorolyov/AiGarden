using Amazon.Runtime;
using Amazon.S3;
using AiGarden.BuildingBlocks.Extensions;
using AiGarden.StorageS3Service.Core.Repositories;
using AiGarden.StorageS3Service.Core.Services;
using AiGarden.StorageS3Service.Infrastructure.Persistence;
using AiGarden.StorageS3Service.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AiGarden.StorageS3Service.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddStorageInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SeaweedFsS3Options>(configuration.GetSection(SeaweedFsS3Options.SectionName));

        services.AddDbContext<StorageDbContext>(options =>
            options.UseNpgsql(configuration.GetRequiredConnectionString("StorageDb")));

        services.AddScoped<IStoredPlantPhotoRepository, StoredPlantPhotoRepository>();

        services.AddSingleton<IAmazonS3>(_ =>
        {
            var options = configuration.GetSection(SeaweedFsS3Options.SectionName).Get<SeaweedFsS3Options>()
                ?? new SeaweedFsS3Options();

            return new AmazonS3Client(
                new BasicAWSCredentials(options.AccessKey, options.SecretKey),
                new AmazonS3Config
                {
                    ServiceURL = options.ServiceUrl,
                    ForcePathStyle = options.ForcePathStyle
                });
        });

        services.AddScoped<IObjectStorage, SeaweedFsS3Storage>();
        return services;
    }
}
