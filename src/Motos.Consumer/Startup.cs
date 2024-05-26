using Microsoft.EntityFrameworkCore;
using Motos.Data;

namespace Motos.Consumer;

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

        services.AddSingleton<IMotosRepository, MotosRepository>();
        services.AddDbContext<MotosContext>(options =>
            options.UseNpgsql(Environment.GetEnvironmentVariable("ConnectionString__DefaultConnection")));

        services.AddHostedService<RabbitMQBackgroundService>(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<RabbitMQBackgroundService>>();
            var serviceScopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
            var rabbitMQHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST");
            var rabbitMQPort = int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT"));
            var rabbitMQUser = Environment.GetEnvironmentVariable("RABBITMQ_USER");
            var rabbitMQPassword = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD");

            return new RabbitMQBackgroundService(
                logger,
                serviceScopeFactory,
                rabbitMQHost,
                rabbitMQPort,
                rabbitMQUser,
                rabbitMQPassword
            );
        });
    }


}
