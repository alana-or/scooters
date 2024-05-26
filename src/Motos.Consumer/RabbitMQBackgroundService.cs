using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Motos.Data.Entities;
using Motos.Data;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

public class RabbitMQBackgroundService : BackgroundService
{
    private readonly ILogger<RabbitMQBackgroundService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly string _rabbitMQHost;
    private readonly int _rabbitMQPort;
    private readonly string _rabbitMQUser;
    private readonly string _rabbitMQPassword;
    private IConnection _connection;
    private IModel _channel;

    public RabbitMQBackgroundService(
        ILogger<RabbitMQBackgroundService> logger,
        IServiceScopeFactory serviceScopeFactory,
        string rabbitMQHost,
        int rabbitMQPort,
        string rabbitMQUser,
        string rabbitMQPassword)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _rabbitMQHost = rabbitMQHost;
        _rabbitMQPort = rabbitMQPort;
        _rabbitMQUser = rabbitMQUser;
        _rabbitMQPassword = rabbitMQPassword;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.Register(() => _logger.LogInformation("RabbitMQ background task is stopping."));

        return Task.Run(async () => {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (_connection == null || !_connection.IsOpen)
                    {
                        ConnectToRabbitMQ();
                    }

                    if (_channel == null || !_channel.IsOpen)
                    {
                        _channel = _connection.CreateModel();
                        _channel.QueueDeclare(queue: "motos_queue",
                                             durable: true,
                                             exclusive: false,
                                             autoDelete: false,
                                             arguments: null);

                        var consumer = new EventingBasicConsumer(_channel);
                        consumer.Received += async (model, ea) =>
                        {
                            var body = ea.Body.ToArray();
                            var message = Encoding.UTF8.GetString(body);
                            _logger.LogInformation($"Received message: {message}");

                            using (var scope = _serviceScopeFactory.CreateScope())
                            {
                                var motosRepository = scope.ServiceProvider.GetRequiredService<IMotosRepository>();
                                await motosRepository.SaveMoto2024Async(new MotosLog2024(message));
                            }

                            // Acknowledge the message
                            _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                        };

                        _channel.BasicConsume(queue: "motos_queue",
                                             autoAck: false,
                                             consumer: consumer);
                    }

                    await Task.Delay(5000, stoppingToken); // Wait for 5 seconds before checking again
                }
                catch (BrokerUnreachableException ex)
                {
                    _logger.LogError(ex, "RabbitMQ BrokerUnreachableException: Unable to reach RabbitMQ broker. Retrying in 5 seconds...");
                    await Task.Delay(5000, stoppingToken); // Wait before retrying to connect
                }
                catch (AlreadyClosedException ex)
                {
                    _logger.LogError(ex, "RabbitMQ connection was closed. Reconnecting...");
                    DisposeConnectionAndChannel();
                    await Task.Delay(5000, stoppingToken); // Wait before retrying to connect
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An unexpected error occurred.");
                    await Task.Delay(5000, stoppingToken); // Wait before retrying on general error
                }
            }

            DisposeConnectionAndChannel();
        }, stoppingToken);
    }

    private void ConnectToRabbitMQ()
    {
        var factory = new ConnectionFactory()
        {
            HostName = _rabbitMQHost,
            Port = _rabbitMQPort,
            UserName = _rabbitMQUser,
            Password = _rabbitMQPassword,
        };

        _connection = factory.CreateConnection();
        _logger.LogInformation("RabbitMQ connection established.");
    }

    private void DisposeConnectionAndChannel()
    {
        if (_channel != null)
        {
            _channel.Close();
            _channel.Dispose();
            _channel = null;
        }

        if (_connection != null)
        {
            _connection.Close();
            _connection.Dispose();
            _connection = null;
        }
    }

    public override void Dispose()
    {
        DisposeConnectionAndChannel();
        base.Dispose();
    }
}
