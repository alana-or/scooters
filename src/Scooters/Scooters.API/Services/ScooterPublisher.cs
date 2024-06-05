using Newtonsoft.Json;
using RabbitMQ.Client;
using Serilog;
using System.Text;

namespace Scooters.Api.Services;

public class ScooterPublisher : IEventPublisher
{
    private readonly RabbitMqConfig _config;
    private readonly Serilog.ILogger _logger;

    public ScooterPublisher(RabbitMqConfig config)
    {
        _logger = Log.ForContext<ScooterPublisher>();
        _config = config;
    }

    public void Publish<T>(T eventMessage) where T : class
    {
        var factory = new ConnectionFactory
        {
            HostName = _config.HostName,
            UserName = _config.Username,
            Password = _config.Password,
            Port = _config.Port
        };

        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();
        channel.QueueDeclare(queue: _config.QueueName,
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var message = JsonConvert.SerializeObject(eventMessage);
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "",
                             routingKey: _config.QueueName,
                             basicProperties: null,
                             body: body);
        
        _logger.Information($"Send message {message}");
    }
}
