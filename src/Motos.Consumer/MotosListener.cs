using Motos.Data;
using Motto.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Motos.Consumer;

public class MotosListener : IDisposable
{
    private readonly IMotosRepository motosRepository;
    private readonly string _hostname;
    private readonly string _queueName;
    private readonly string _username;
    private readonly string _password;
    private IConnection _connection;
    private IModel _channel;

    public MotosListener(string hostname, string queueName, string username, string password, IMotosRepository motosRepository)
    {
        _hostname = hostname;
        _queueName = queueName;
        _username = username;
        _password = password;

        this.motosRepository = motosRepository;

        InitializeRabbitMQListener();
    }

    private void InitializeRabbitMQListener()
    {
        var factory = new ConnectionFactory
        {
            HostName = _hostname,
            UserName = _username,
            Password = _password
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: _queueName,
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);
    }

    public void StartListening()
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($" [x] Received {message}");

            motosRepository.SaveMoto2024(new MotosLog2024(message));
        };
        _channel.BasicConsume(queue: _queueName,
                             autoAck: true,
                             consumer: consumer);
    }

    public void Dispose()
    {
        _channel.Close();
        _connection.Close();
    }
}
