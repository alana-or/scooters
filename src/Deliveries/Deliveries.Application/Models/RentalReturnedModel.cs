using Deliveries.Domain;

namespace Deliveries.Application.Models;

public class RentalReturnedModel
{
    public Guid Id { get; set; }
    public Guid ScooterId { get; set; }
    public int Year { get; set; }
    public double RentTotal { get; set; }
    public string Model { get; set; }
    public string LicencePlate { get; set; }
    public DeliveryPerson DeliveryPerson { get; set; }
    public DateTime Create { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public DateTime EndExpected { get; set; }
}