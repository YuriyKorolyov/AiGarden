using AiGarden.BuildingBlocks.Abstractions;
using AiGarden.HistoryService.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace AiGarden.HistoryService.Infrastructure.Persistence;

public sealed class HistoryDbContext(DbContextOptions<HistoryDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<AnalysisHistoryEntry> HistoryEntries => Set<AnalysisHistoryEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AnalysisHistoryEntry>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.AnalysisId).IsUnique();
            builder.Property(x => x.PhotoUrl).HasMaxLength(2048).IsRequired();
            builder.Property(x => x.Provider).HasMaxLength(128).IsRequired();
            builder.Property(x => x.Model).HasMaxLength(256).IsRequired();
            builder.Property(x => x.Diagnosis).HasColumnType("text");
        });
    }
}
