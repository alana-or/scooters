using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Scooters.Data;

class Program
{
    static void Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddLogging(config =>
            {
                config.SetMinimumLevel(LogLevel.Information);
            })
            .AddDbContext<ScootersContext>(options =>
                options.UseNpgsql("Host=scooters_db;Port=5432;Username=postgres;Password=postgrespw;Database=scooters_db;Search Path=public"))
            .BuildServiceProvider();

        using (var _context = serviceProvider.GetRequiredService<ScootersContext>())
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