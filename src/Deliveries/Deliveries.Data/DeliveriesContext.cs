using Deliveries.Application.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Deliveries.Data;

public class DeliveriesContext : DbContext
{
    public DeliveriesContext() { }
    public DeliveriesContext(DbContextOptions<DeliveriesContext> options)
        : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public DbSet<DeliveryPersonDb> DeliveryPeople { get; set; }
    public DbSet<DeliveryPersonRentalDb> DeliveryPersonRentals { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=deliveries_db;Port=5432;Username=postgres;Password=postgrespw;Database=deliveries_db;Search Path=public");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DeliveryPersonDb>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Name).IsRequired().HasMaxLength(20);
            entity.Property(m => m.CNHImage).IsRequired();
            entity.HasIndex(m => m.CNPJ).IsUnique();
            entity.HasIndex(m => m.CNH).IsUnique();
            entity.Property(m => m.CNHType).IsRequired();
            entity.Property(m => m.Birth).IsRequired()
                .HasColumnType("timestamp with time zone");
        });

        modelBuilder.Entity<DeliveryPersonRentalDb>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Year).IsRequired().HasMaxLength(4);
            entity.Property(m => m.ScooterId).IsRequired();
            entity.Property(m => m.LicencePlate).IsRequired();
            entity.Property(m => m.Model).IsRequired();
        });
    }
}
