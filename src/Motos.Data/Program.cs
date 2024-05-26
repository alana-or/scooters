using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Motos.Data;

class Program
{
    static void Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddLogging(config =>
            {
                config.SetMinimumLevel(LogLevel.Information);
            })
            .AddDbContext<MotosContext>(options =>
                options.UseNpgsql("Host=motos_db;Port=5432;Username=postgres;Password=postgrespw;Database=motos_db;Search Path=public"))
            .BuildServiceProvider();

        using (var context = serviceProvider.GetRequiredService<MotosContext>())
        {
            try
            {
                context.Database.Migrate();
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