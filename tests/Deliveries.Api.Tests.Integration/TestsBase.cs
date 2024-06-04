using Deliveries.Data.Entities;

[TestFixture]
public class TestsBase
{
    public HttpClient Client { get; set; }
    public IEnumerable<DeliveryPersonRentalDb> DeliveryPersonRentals { get; set; }

    [OneTimeSetUp]
    public async Task SetUp()
    {
        Client = ApiFixture.HttpClient;
        DeliveryPersonRentals = ApiFixture.DeliveryPersonRentals;
    }
}

