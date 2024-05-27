using Microsoft.EntityFrameworkCore;
using Motos.Data.Entities;

namespace Motos.Data;

public class MotosContext : DbContext
{
    public MotosContext() { }
    public MotosContext(DbContextOptions<MotosContext> options)
        : base(options)
    {

    }

    public DbSet<MotoDB> Motos { get; set; }
    public DbSet<MotosLog2024> MotosLog2024 { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=motos_db;Port=5432;Username=postgres;Password=postgrespw;Database=motos_db;Search Path=public");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MotoDB>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Modelo).IsRequired().HasMaxLength(100);
            entity.Property(m => m.Placa).IsRequired().HasMaxLength(20);
            entity.Property(m => m.Ano).IsRequired();
        });

        modelBuilder.Entity<MotosLog2024>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Message).IsRequired().HasMaxLength(1000);
        });
    }
}
