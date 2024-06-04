namespace Deliveries.Api.Domain;

public class DeliveryPersonRental
{
    public Guid Id { get; set; }
    public Guid ScooterId { get; set; }
    public int Year { get; set; }
    public string Model { get; set; }
    public string LicencePlate { get; set; }
    public DeliveryPerson DeliveryPerson {  get; set; }

    public DeliveryPersonRental(Guid scooterId, int year, string model,
        string licencePlate, DeliveryPerson deliveryPerson)
    {
        Id = new Guid();
        ScooterId = scooterId;
        Year = year;
        Model = model;
        LicencePlate = licencePlate;
        DeliveryPerson = deliveryPerson;
    }
}