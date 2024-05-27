using Motos.API.Services;
using Newtonsoft.Json;

namespace Motos.API.Domain;

public class Moto
{
    public int Id { get; set; }

    public int Ano { get; set; }

    public string Modelo { get; set; }

    public string Placa { get; set; }

    public void Publish()
    {
        if(Ano == 2024)
        {
            var publisher = new MotosPublisher(hostname: "motos_rabbit",
                queueName: "motos_queue",
                username: "guest",
                password: "guest",
                port: 5672);

            string message = JsonConvert.SerializeObject(this);
            publisher.PublishMessage(message);
            publisher.Dispose();
        }
    }
}
