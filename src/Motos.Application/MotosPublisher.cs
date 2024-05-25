using RabbitMQ.Client;
using System.Text;

public class MotosPublisher : IDisposable
{
    private readonly string _hostname;
    private readonly string _queueName;
    private readonly string _username;
    private readonly string _password;
    private readonly int _port;
    private IConnection _connection;
    private RabbitMQ.Client.IModel _channel;

    public MotosPublisher(string hostname, string queueName, string username, string password, int port)
    {
        _hostname = hostname;
        _queueName = queueName;
        _username = username;
        _password = password;
        _port = port;

        InitializeRabbitMQPublisher();
    }

    private void InitializeRabbitMQPublisher()
    {
        var factory = new ConnectionFactory
        {
            HostName = _hostname,
            UserName = _username,
            Password = _password,
            Port = _port
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: _queueName,
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);
    }

    public void PublishMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(exchange: "",
                             routingKey: _queueName,
                             basicProperties: null,
                             body: body);

        Console.WriteLine($" [x] Sent {message}");
    }

    public void Dispose()
    {
        _channel?.Close();
        _channel?.Dispose();
        _connection?.Close();
        _connection?.Dispose();
    }
}
