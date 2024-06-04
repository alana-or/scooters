namespace Deliveries.Api.Models;

public class RentalCreate
{
    public ScooterResponse Scooter { get; set; }
    public DeliveryPersonResponse DeliveryPerson {  get; set; }
}