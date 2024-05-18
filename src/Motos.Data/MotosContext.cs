using Microsoft.EntityFrameworkCore;
using Motto.Entities;

namespace Motos.Data;

public class MotosContext : DbContext
{
    public MotosContext(DbContextOptions<MotosContext> options)
        : base(options)
    {

    }

    public DbSet<Moto> Motos { get; set; }
    public DbSet<MotosLog2024> MotosLog2024 { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Moto>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Modelo).IsRequired().HasMaxLength(100);
            entity.Property(m => m.Placa).IsRequired().HasMaxLength(20);
            entity.Property(m => m.Ano).IsRequired();
        });

        modelBuilder.Entity<MotosLog2024>(entity =>
        {
            entity.Property(m => m.Message).IsRequired().HasMaxLength(1000);
        });
    }
}
