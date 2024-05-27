using Scooters.Api.Services;
using Newtonsoft.Json;

namespace Scooters.Api.Domain;

public class Scooter
{
    public int Id { get; set; }

    public int Year { get; set; }

    public string Model { get; set; }

    public string LicencePlate { get; set; }

    public void Publish()
    {
        if(Year == 2024)
        {
            var publisher = new ScooterPublisher(hostname: "scooters_rabbit",
                queueName: "scooters_queue",
                username: "guest",
                password: "guest",
                port: 5672);

            string message = JsonConvert.SerializeObject(this);
            publisher.PublishMessage(message);
            publisher.Dispose();
        }
    }
}
