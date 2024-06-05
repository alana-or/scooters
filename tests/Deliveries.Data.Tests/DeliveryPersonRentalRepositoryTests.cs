using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Deliveries.Data.Builders;
using Deliveries.Data.Entities;
using Testcontainers.PostgreSql;

namespace Deliveries.Data.Tests;

[TestFixture]
public class DeliveryPersonRentalRepositoryTests
{
    private PostgreSqlContainer _postgresContainer;
    private ServiceProvider _serviceProvider;
    private Mock<ILogger<DeliveryPersonRentalsRepository>> _loggerMock;

    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        _postgresContainer = new PostgreSqlBuilder()
            .WithDatabase("deliveries_db")
            .WithUsername("postgres")
            .WithPassword("postgrespw")
            .WithCleanUp(true)
            .Build();

        _loggerMock = new Mock<ILogger<DeliveryPersonRentalsRepository>>();
        _loggerMock.SetupAllProperties();

        await _postgresContainer.StartAsync();

        var services = new ServiceCollection();

        services.AddSingleton(_loggerMock.Object);

        services.AddLogging(config =>
        {
            config.SetMinimumLevel(LogLevel.Information);
        });

        services.AddDbContext<DeliveriesContext>(options =>
            options.UseNpgsql(_postgresContainer.GetConnectionString()));
        services.AddScoped<IDeliveryPersonRentalsRepository, DeliveryPersonRentalsRepository>();

        _serviceProvider = services.BuildServiceProvider();

        using (var _scope = _serviceProvider.CreateScope())
        {
            var _context = _scope.ServiceProvider.GetRequiredService<DeliveriesContext>();
            _context.Database.Migrate();
        }
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _postgresContainer.DisposeAsync();
        await _serviceProvider.DisposeAsync();
    }

    [Test]
    public async Task Create_Should_Create_DeliveryPersonRental()
    {
        using (var _scope = _serviceProvider.CreateScope())
        {
            var _deliveries = _scope.ServiceProvider.GetRequiredService<IDeliveryPersonRentalsRepository>();
            var _context = _scope.ServiceProvider.GetRequiredService<DeliveriesContext>();
            var person = new DeliveryPersonBuilder().Build();
            _context.DeliveryPeople.Add(person);
            await _context.SaveChangesAsync();

            var delivery = new DeliveryPersonRentalDb { 
                Id = new Guid(), 
                DeliveryPersonId = person.Id,
                DeliveryPerson = person,
                LicencePlate = "LicencePlate",
                Model = "Model",
                Year = 2023,
                ScooterId = new Guid(),
            };

            await _deliveries.CreateAsync(delivery);

            var updatedDeliveryRental = await _context.DeliveryPersonRentals.FindAsync(delivery.Id);

            updatedDeliveryRental.Model.Should().Be("Model");
            updatedDeliveryRental.Year.Should().Be(2023);
            updatedDeliveryRental.LicencePlate.Should().Be("LicencePlate");
            updatedDeliveryRental.DeliveryPerson.Name.Should().Be(person.Name);
            updatedDeliveryRental.DeliveryPerson.CNHImage.Should().Be(person.CNHImage);
        }
    }

    [Test]
    public async Task Create_Should_Log_Error_When_Exception_Occurs()
    {
        using (var _scope = _serviceProvider.CreateScope())
        {
            var _deliveries = _scope.ServiceProvider.GetRequiredService<IDeliveryPersonRentalsRepository>();

            DisposeDataBase(_scope);

            Func<Task> act = async () => await _deliveries.CreateAsync(new DeliveryPersonRentalDb());

            await act.Should().ThrowAsync<Exception>();
            VerifyThrowExeption("An error occurred while creating delivery rental.", LogLevel.Error, Times.Once());
        }
    }

    [Test]
    public async Task Update_Should_Update_DeliveryPersonRental()
    {
        using (var _scope = _serviceProvider.CreateScope())
        {
            var _deliveries = _scope.ServiceProvider.GetRequiredService<IDeliveryPersonRentalsRepository>();
            var _context = _scope.ServiceProvider.GetRequiredService<DeliveriesContext>();

            var delivery = new DeliveryPersonRentalDb
            {
                Id = new Guid(),
                DeliveryPerson = new DeliveryPersonBuilder().Build(),
                LicencePlate = "LicencePlate",
                Model = "Model",
                Year = 2023,
                ScooterId = new Guid(),
            };

            _context.DeliveryPersonRentals.Add(delivery);
            await _context.SaveChangesAsync();

            delivery.LicencePlate = "New LicencePlate";
            delivery.Model = "New Model";
            await _deliveries.UpdateAsync(delivery);

            var updatedDeliveryPerson = await _context
                .DeliveryPersonRentals
                .FindAsync(delivery.Id);

            updatedDeliveryPerson.LicencePlate.Should().Be("New LicencePlate");
            updatedDeliveryPerson.Model.Should().Be("New Model");
        }
    }

    [Test]
    public async Task Update_Should_Log_Error_When_Exception_Occurs()
    {
        using (var _scope = _serviceProvider.CreateScope())
        {
            var _deliveries = _scope.ServiceProvider.GetRequiredService<IDeliveryPersonRentalsRepository>();

            DisposeDataBase(_scope);

            Func<Task> act = async () => await _deliveries.UpdateAsync(new DeliveryPersonRentalDb());

            await act.Should().ThrowAsync<Exception>();
            VerifyThrowExeption("An error occurred while updating delivery rental.", LogLevel.Error, Times.Once());
        }
    }

    private static void DisposeDataBase(IServiceScope _scope)
    {
        var _context = _scope.ServiceProvider.GetRequiredService<DeliveriesContext>();
        _context.Dispose();
    }

    private void VerifyThrowExeption(string message, LogLevel logLevel, Times times)
    {
        _loggerMock.Verify(l => l.Log(
            logLevel,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(message)),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()), times);
    }
}