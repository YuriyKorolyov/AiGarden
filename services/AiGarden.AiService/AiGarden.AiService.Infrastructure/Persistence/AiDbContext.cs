using AiGarden.AiService.Core.Entities;
using AiGarden.BuildingBlocks.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace AiGarden.AiService.Infrastructure.Persistence;

public sealed class AiDbContext(DbContextOptions<AiDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<PlantAnalysis> PlantAnalyses => Set<PlantAnalysis>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PlantAnalysis>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.PhotoUrl).HasMaxLength(2048).IsRequired();
            builder.Property(x => x.Model).HasMaxLength(256).IsRequired();
            builder.Property(x => x.Diagnosis).HasColumnType("text");
            builder.Property(x => x.Error).HasColumnType("text");
        });
    }
}
