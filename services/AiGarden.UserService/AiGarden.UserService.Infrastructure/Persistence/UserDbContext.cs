using AiGarden.BuildingBlocks.Abstractions;
using AiGarden.UserService.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace AiGarden.UserService.Infrastructure.Persistence;

public sealed class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<AppUser> Users => Set<AppUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Subject).IsUnique();
            builder.Property(x => x.Subject).HasMaxLength(256).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(256);
            builder.Property(x => x.DisplayName).HasMaxLength(256);
        });
    }
}
