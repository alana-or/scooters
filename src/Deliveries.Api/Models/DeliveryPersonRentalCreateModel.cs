namespace Deliveries.Api.Models;

public class DeliveryPersonRentalCreateModel
{
    public ScooterModel Scooter { get; set; }
    public Guid DeliveryPersonId {  get; set; }
    public DateTime EndExpected {  get; set; }
}