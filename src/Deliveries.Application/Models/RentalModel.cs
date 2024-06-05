namespace Deliveries.Application.Models;

public class RentalModel
{
    public Guid Id { get; set; }
    public ScooterModel Scooter { get; set; }
    public DateTime EndExpected {  get; set; }
    public DeliveryPersonModel DeliveryPerson {  get; set; }
}