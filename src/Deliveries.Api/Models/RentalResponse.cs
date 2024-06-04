namespace Deliveries.Api.Models;

public class RentalResponse
{
    public Guid Id { get; set; }
    public ScooterResponse Scooter { get; set; }
    public DeliveryPersonResponse DeliveryPerson {  get; set; }
}