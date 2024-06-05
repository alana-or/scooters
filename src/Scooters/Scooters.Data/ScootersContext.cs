using Microsoft.EntityFrameworkCore;
using Scooters.Data.Entities;

namespace Scooters.Data;

public class ScootersContext : DbContext
{
    public ScootersContext() { }
    public ScootersContext(DbContextOptions<ScootersContext> options)
        : base(options)
    {

    }

    public DbSet<ScooterDB> Scooters { get; set; }
    public DbSet<ScooterLog2024> ScootersLog2024 { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=scooters_db;Port=5432;Username=postgres;Password=postgrespw;Database=scooters_db;Search Path=public");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ScooterDB>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Model).IsRequired().HasMaxLength(100);
            entity.Property(m => m.LicencePlate).IsRequired().HasMaxLength(20);
            entity.Property(m => m.Year).IsRequired();
        });

        modelBuilder.Entity<ScooterLog2024>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Message).IsRequired().HasMaxLength(1000);
        });
    }
}
