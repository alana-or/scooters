﻿using Motos.Data;
using Motos.Data.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

public class RabbitMQBackgroundService : BackgroundService
{
    private readonly ILogger<RabbitMQBackgroundService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly string _rabbitMQHost;
    private readonly int _rabbitMQPort;
    private readonly string _rabbitMQUser;
    private readonly string _rabbitMQPassword;
    private readonly string _rabbitMQQueue;
    private IConnection _connection;
    private IModel _channel;
    private const int _intervaloMensagemWorkerAtivo = 5000;

    public RabbitMQBackgroundService(
        ILogger<RabbitMQBackgroundService> logger,
        IServiceScopeFactory serviceScopeFactory,
        string rabbitMQHost,
        string rabbitMQUser,
        string rabbitMQPassword,
        int rabbitMQPort,
        string rabbitMQQueue
        )
    {

        _logger = logger;

        _serviceScopeFactory = serviceScopeFactory;
        _rabbitMQHost = rabbitMQHost;
        _rabbitMQPort = rabbitMQPort;
        _rabbitMQUser = rabbitMQUser;
        _rabbitMQPassword = rabbitMQPassword;
        _rabbitMQQueue = rabbitMQQueue;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Aguardando mensagens...");

        var factory = new ConnectionFactory()
        {
            HostName = _rabbitMQHost,
            Port =  _rabbitMQPort,
            UserName = _rabbitMQUser,
            Password = _rabbitMQPassword,
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: _rabbitMQQueue,
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

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var motosRepository = scope.ServiceProvider.GetRequiredService<IMotosRepository>();
                await motosRepository.SaveMoto2024Async(new MotosLog2024(message));
            }

        }; 

        channel.BasicConsume(queue: _rabbitMQQueue,
            autoAck: true,
            consumer: consumer);

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation(
                $"Worker ativo em: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            await Task.Delay(_intervaloMensagemWorkerAtivo, stoppingToken);
        }
    }
}
