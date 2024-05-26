using Motos.Data;
using Motos.Data.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Motos.Consumer;

public class RabbitMQBackgroundService : BackgroundService
{
    private readonly ILogger<RabbitMQBackgroundService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly string _rabbitMQHost;
    private readonly int _rabbitMQPort;
    private readonly string _rabbitMQUser;
    private readonly string _rabbitMQPassword;
    private readonly IMotosRepository motosRepository;

    public RabbitMQBackgroundService(
        ILogger<RabbitMQBackgroundService> logger,
        IServiceScopeFactory serviceScopeFactory,
        IMotosRepository motosRepository)
    {

        var rabbitMQHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST");
        var rabbitMQPort = int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT"));
        var rabbitMQUser = Environment.GetEnvironmentVariable("RABBITMQ_USER");
        var rabbitMQPassword = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD");

        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _rabbitMQHost = rabbitMQHost;
        _rabbitMQPort = rabbitMQPort;
        _rabbitMQUser = rabbitMQUser;
        _rabbitMQPassword = rabbitMQPassword;
        this.motosRepository = motosRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory()
        {
            HostName = _rabbitMQHost,
            Port = _rabbitMQPort,
            UserName = _rabbitMQUser,
            Password = _rabbitMQPassword,
        };

        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "motos_queue",
                                    durable: true,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation($"Received message: {message}");

                await motosRepository.SaveMoto2024Async(new MotosLog2024(message));

                // Acknowledge the message
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            await motosRepository.SaveMoto2024Async(new MotosLog2024("message"));

            channel.BasicConsume(queue: "motos_queue",
                                    autoAck: true,
                                    consumer: consumer);

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken); // Wait for 5 seconds before reconnecting
        }
    }
}