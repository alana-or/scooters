namespace Scooters.Api.Services;

public class RabbitMqConfig
{
    public string HostName { get; set; } = "scooters_rabbit";
    public string QueueName { get; set; } = "scooters_queue";
    public string Username { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public int Port { get; set; } = 5672;
}