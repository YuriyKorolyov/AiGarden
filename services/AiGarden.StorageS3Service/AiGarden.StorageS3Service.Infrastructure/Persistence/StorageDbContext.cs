using AiGarden.BuildingBlocks.Abstractions;
using AiGarden.StorageS3Service.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace AiGarden.StorageS3Service.Infrastructure.Persistence;

public sealed class StorageDbContext(DbContextOptions<StorageDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<StoredPlantPhoto> StoredPlantPhotos => Set<StoredPlantPhoto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StoredPlantPhoto>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.FileName).HasMaxLength(255).IsRequired();
            builder.Property(x => x.ContentType).HasMaxLength(128).IsRequired();
            builder.Property(x => x.BlobKey).HasMaxLength(512).IsRequired();
            builder.Property(x => x.PublicUrl).HasMaxLength(2048).IsRequired();
        });
    }
}
