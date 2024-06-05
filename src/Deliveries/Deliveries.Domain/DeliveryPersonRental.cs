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
    public DateTime EndExpected { get; set; }

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
        EndExpected = expectedDate;
    }

    public void CalculateRent()
    {
        var today = DateTime.Today;
        int rentDays = CalculateRentDays(today);
        int excessDays = CalculateExcessDays(today, rentDays);

        RentTotal = new RentCalculator().CalculateRent(rentDays, excessDays);
    }

    private int CalculateExcessDays(DateTime today, int rentDays)
    {
        TimeSpan difference = today - EndExpected;
        int differenceExpected = rentDays == 0 ? difference.Days + 1 : difference.Days;
        return differenceExpected;
    }

    private int CalculateRentDays(DateTime today)
    {
        TimeSpan differenceRent = today - Start;
        int rentDays = differenceRent.Days > 0 ?
            differenceRent.Days : 0;
        return rentDays;
    }
}