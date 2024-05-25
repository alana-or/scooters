using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Motos.Data;
using Motto.Entities;

namespace Motos.Consumer;

public class MotosListener : IDisposable
{
    private readonly string _hostname;
    private readonly string _queueName;
    private readonly string _username;
    private readonly string _password;
    private readonly int _port;
    private readonly IMotosRepository _motosRepository;
    private readonly ILogger<MotosListener> _logger;
    private IConnection _connection;
    private IModel _channel;

    public MotosListener(string hostname, string queueName, string username, int port, string password, IMotosRepository motosRepository, ILogger<MotosListener> logger)
    {
        _hostname = hostname;
        _queueName = queueName;
        _username = username;
        _password = password;
        _motosRepository = motosRepository;
        _logger = logger;
        _port = port;
        InitializeRabbitMqListener();
    }

    private void InitializeRabbitMqListener()
    {
        var factory = new ConnectionFactory
        {
            HostName = _hostname,
            UserName = _username,
            Password = _password,
            Port = (int)_port
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(
            queue: _queueName, 
            durable: true, 
            exclusive: false, 
            autoDelete: false, 
            arguments: null);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation($"Received message: {message}");

            _motosRepository.SaveMoto2024(new MotosLog2024(message));
        };

        _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
    }

    public void StartListening()
    {
        _logger.LogInformation("Started listening to RabbitMQ queue.");
    }

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }
}
