namespace Deliveries.Api.Models;

public class Rental
{
    public Scooter Scooter { get; set; }
    public DeliveryPersonResponse DeliveryPerson {  get; set; }
}