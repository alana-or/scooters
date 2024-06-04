namespace Deliveries.Api.Models;

public class ScooterModel
{
    public Guid Id { get; set; }

    public int Year { get; set; }

    public string Model { get; set; }

    public string LicencePlate { get; set; }
}