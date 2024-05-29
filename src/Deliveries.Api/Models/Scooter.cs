namespace Deliveries.Api.Models;

public class Scooter
{
    public Guid Id { get; set; }

    public int Year { get; set; }

    public string Model { get; set; }

    public string LicencePlate { get; set; }
}