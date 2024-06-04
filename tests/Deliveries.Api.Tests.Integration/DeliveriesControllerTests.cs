using Deliveries.Api.Models;
using FluentAssertions;
using Newtonsoft.Json;
using System.Text;

namespace Deliveries.Tests.Controllers;

public class DeliveriesControllerTests : BaseTests
{
    [Test]
    public async Task CreateRental_ShouldReturnOk()
    {
        var rental = new RentalCreate
        {
            DeliveryPerson = new DeliveryPersonResponse
            {
                Id = Guid.NewGuid(),
                Name = "name",
                Photo = "photo"
            },
            Scooter = new ScooterResponse
            {
                Id = Guid.NewGuid(),
                LicencePlate = "LicencePlate",
                Model = "Model",
                Year = 2024
            }
        };

        var content = new StringContent(JsonConvert.SerializeObject(rental), Encoding.UTF8, "application/json");

        var response = await Client.PostAsync("v1/api/deliveries/rentals/create", content);

        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<RentalResponse>(responseString);

        responseObject.Should().BeEquivalentTo(
            rental,
            cfg => cfg
                .Excluding(a => a.Scooter.Id)
                .Excluding(a => a.DeliveryPerson.Id)
        );
    }
}
