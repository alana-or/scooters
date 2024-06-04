namespace Deliveries.Api.Models;

public class RentalModel
{
    public Guid Id { get; set; }
    public ScooterModel Scooter { get; set; }
    public DeliveryPersonModel DeliveryPerson {  get; set; }
}