using Microsoft.EntityFrameworkCore;
using Deliveries.Data.Entities;

namespace Deliveries.Data;

public class DeliveriesContext : DbContext
{
    public DeliveriesContext() { }
    public DeliveriesContext(DbContextOptions<DeliveriesContext> options)
        : base(options)
    {

    }

    public DbSet<DeliveryPersonDb> DeliveryPeople { get; set; }
    public DbSet<DeliveriesRentalDb> DeliveriesRental { get; set; }

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
            entity.Property(m => m.Photo).IsRequired();
        });

        modelBuilder.Entity<DeliveriesRentalDb>(entity =>
        {
            entity.HasKey(m => m.Id);
        });
    }
}
