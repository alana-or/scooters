using Deliveries.Api.Models;
using FluentAssertions;
using FluentAssertions.Extensions;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Deliveries.Tests.Controllers;

public class DeliveriesControllerTests : TestsBase
{
    [Test]
    public async Task CreateRentalAsync_ShouldReturnOk()
    {
        var personId = DeliveryPersonRentals.First().DeliveryPerson.Id;

        var rental = new DeliveryPersonRentalCreateModel
        {
            DeliveryPersonId = personId,
            Scooter = new ScooterModel
            {
                Id = Guid.NewGuid(),
                LicencePlate = "LicencePlate",
                Model = "Model",
                Year = 2024
            }
        };

        var content = new StringContent(JsonConvert.SerializeObject(rental), Encoding.UTF8, "application/json");

        var response = await Client.PostAsync("deliveries/rentals/create", content);

        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<RentalModel>(responseString);

        responseObject.Should().BeEquivalentTo(
            rental,
            cfg => cfg
                .Excluding(a => a.Scooter.Id)
                .Excluding(a => a.DeliveryPersonId)
        );
    }

    [Test]
    public async Task CreateRentalAsync_WithInvalidData_ShouldReturnBadRequest()
    {
        var rental = new DeliveryPersonRentalCreateModel
        {
            DeliveryPersonId = Guid.Empty, 
            Scooter = new ScooterModel
            {
                Id = Guid.NewGuid(),
                LicencePlate = "LicencePlate",
                Model = "Model",
                Year = 2024
            }
        };

        var content = new StringContent(JsonConvert.SerializeObject(rental), Encoding.UTF8, "application/json");

        var response = await Client.PostAsync("deliveries/rentals/create", content);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task GetPersonRentalsAsync_ShouldReturnOk()
    {
        var personId = DeliveryPersonRentals.First().DeliveryPerson.Id;

        var response = await Client.GetAsync($"deliveries/rentals/{personId}");

        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<IEnumerable<RentalModel>>(responseString);
        responseObject.Should().HaveCountGreaterThan(4);
    }

    [Test]
    public async Task CreatePersonAsync_ShouldReturnOk()
    {
        var person = new DeliveryPersonCreateModel
        {
            Name = "name",
            CNHImage = "image",
            Birth = DateTime.Now.AsUtc(),
            CNH = "123",
            CNPJ = "123",
            CNHType = 'A'
        };

        var content = new StringContent(JsonConvert.SerializeObject(person), Encoding.UTF8, "application/json");

        var response = await Client.PostAsync($"deliveries/person/create", content);

        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<DeliveryPersonModel>(responseString);

        responseObject.Should().BeEquivalentTo(person);
    }

    [Test]
    public async Task UpdatePersonAsync_ShouldReturnOk()
    {
        var personId = DeliveryPersonRentals.First().DeliveryPerson.Id;

        var person = new DeliveryPersonUpdateModel
        {
            Id = personId,
            CNHImage = "new image"
        };

        var content = new StringContent(JsonConvert.SerializeObject(person), Encoding.UTF8, "application/json");

        var response = await Client.PutAsync($"deliveries/person/update", content);

        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<DeliveryPersonModel>(responseString);

        responseObject.Should().BeEquivalentTo(person);
    }
}
