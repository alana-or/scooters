using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Deliveries.Api;
using Deliveries.Api.Models;
using Deliveries.Api.Services;
using Deliveries.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.Internal;
using NUnit.Framework;

namespace Deliveries.Tests.Controllers;

[TestFixture]
public class DeliveriesControllerTests : IDisposable
{
    private readonly WebApplicationFactory<Startup> _factory;
    private HttpClient _client;
    private PostgreSqlTestcontainerFixture _containerFixture;

    public DeliveriesControllerTests()
    {
        _factory = new WebApplicationFactory<Startup>();
        _containerFixture = new PostgreSqlTestcontainerFixture();
    }

    [SetUp]
    public void SetUp()
    {
        _client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices((context, services) =>
             {
                 services.AddScoped<DbContext, DeliveriesContext>();

                 services.AddDbContext<DeliveriesContext>(options =>
                     options.UseNpgsql(_containerFixture.ConnectionString).EnableDetailedErrors());

                 services.AddScoped<IDeliveryPersonRentalsRepository, DeliveryPersonRentalsRepository>();
                 services.AddScoped<IDeliveryPersonRepository, DeliveryPersonRepository>();
                 services.AddScoped<IDeliveriesService, DeliveriesService>();

             });
        }).CreateClient();

        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DeliveriesContext>();
        dbContext.Database.Migrate();
    }

    [Test]
    public async Task CreateRental_ShouldReturnOk()
    {
        // Arrange
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

        var content = new StringContent(JsonSerializer.Serialize(rental), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("v1/api/deliveries/rentals/create", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<Response<string>>(responseString);
        Assert.IsTrue(responseObject.Success);
    }

    public void Dispose()
    {
        _containerFixture.DisposeAsync().Wait();
        _client?.Dispose();
    }
}
