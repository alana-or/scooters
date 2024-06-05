using AutoMapper;
using Deliveries.Application.Mappers;
using Deliveries.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

class Program
{
    static void Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddLogging(config =>
            {
                config.SetMinimumLevel(LogLevel.Information);
            })
            .AddAutoMapper(typeof(DeliveriesMapper))
            .AddDbContext<DeliveriesContext>(options =>
                options.UseNpgsql("Host=deliveries_db;Port=5432;Username=postgres;Password=postgrespw;Database=deliveries_db;Search Path=public"))
            .BuildServiceProvider();

        using (var _context = serviceProvider.GetRequiredService<DeliveriesContext>())
        {
            try
            {
                _context.Database.Migrate();
                Console.WriteLine("Database migration completed.");
            }
            catch (Exception ex)
            {
                var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while migrating the database.");
            }
        }
    }
}