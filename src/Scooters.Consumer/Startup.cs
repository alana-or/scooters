using Microsoft.EntityFrameworkCore;
using Scooters.Data;

namespace Scooters.Consumer;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
        => Configuration = configuration;
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(config =>
        {
            config.AddConsole();
            config.SetMinimumLevel(LogLevel.Information);
        });

        services.AddSingleton<IScootersRepository, ScootersRepository>();
        services.AddDbContext<ScootersContext>(options =>
            options.UseNpgsql(Environment.GetEnvironmentVariable("ConnectionString__DefaultConnection")));

        services.AddHostedService<RabbitMQBackgroundService>(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<RabbitMQBackgroundService>>();
            var serviceScopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
            var rabbitMQHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST");
            var rabbitMQPort = int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT"));
            var rabbitMQUser = Environment.GetEnvironmentVariable("RABBITMQ_USER");
            var rabbitMQPassword = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD");
            var rabbitMQQueue = Environment.GetEnvironmentVariable("RABBITMQ_QUEUE");
           
            return new RabbitMQBackgroundService(
                logger,
                serviceScopeFactory,
                rabbitMQHost,
                rabbitMQUser,
                rabbitMQPassword,
                rabbitMQPort,
                rabbitMQQueue
            );
        });
    }
}
