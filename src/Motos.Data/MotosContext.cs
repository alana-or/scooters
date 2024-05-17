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
}
