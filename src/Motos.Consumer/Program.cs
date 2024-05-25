using Microsoft.EntityFrameworkCore;
using Motos.Data;
using RabbitMQ.Client.Exceptions;

namespace Motos.Consumer;

class Program
{
    static void Main(string[] args)
    {
        var rabbitMQHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST");
        var rabbitMQPort = int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT"));
        var rabbitMQUser = Environment.GetEnvironmentVariable("RABBITMQ_USER");
        var rabbitMQPassword = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD");

        var serviceProvider = new ServiceCollection()
            .AddLogging(config =>
            {
                config.AddConsole();
                config.SetMinimumLevel(LogLevel.Information);
            })
            .AddSingleton<IMotosRepository, MotosRepository>()
            .AddDbContext<MotosContext>(options =>
                options.UseNpgsql(Environment.GetEnvironmentVariable("ConnectionString__DefaultConnection")))
            .AddSingleton<MotosListener>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<MotosListener>>();
                var motosRepository = provider.GetRequiredService<IMotosRepository>();
                return new MotosListener(
                    hostname: rabbitMQHost,
                    queueName: "motos_queue",
                    username: rabbitMQUser,
                    port: rabbitMQPort,
                    password: rabbitMQPassword,
                    motosRepository: motosRepository,
                    logger: logger
                );
            })
            .BuildServiceProvider();

        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

        const int maxRetries = 2;
        int retries = 0;

        while (retries < maxRetries)
        {
            try
            {
                using (var listener = serviceProvider.GetRequiredService<MotosListener>())
                {
                    listener.StartListening();
                    logger.LogInformation("MotosListener started. ");
                }
                break;
            }
            catch (BrokerUnreachableException ex)
            {
                retries++;
                logger.LogError(ex, $"Attempt {retries} of {maxRetries}: Unable to reach RabbitMQ broker. Retrying in 5 seconds...");
                Thread.Sleep(5000);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred.");
                break;
            }
        }
    }
}