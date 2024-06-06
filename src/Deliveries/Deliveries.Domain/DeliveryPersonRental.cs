using Deliveries.Domain.RentCalculators;

namespace Deliveries.Domain;

public class DeliveryPersonRental
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
    public DateTime ExpectedEnd { get; set; }

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

    public void CreateRental(DateTime expectedDate)
    {
        if (DeliveryPerson.CNHType != CNH.A)
        {
            throw new Exception("CNH type A only");
        }

        Create = DateTime.Today;
        Start = DateTime.Today.AddDays(1);
        ExpectedEnd = expectedDate;
        
        CalculateRent(expectedDate);
    }

    public void ReturnRentedScooter()
    {
        End = DateTime.Today; 

        CalculateRent(End);
    }

    private void CalculateRent(DateTime finalDate)
    {
        RentTotal = new RentCalculator().CalculateRent(Start, finalDate, ExpectedEnd);
    }
}