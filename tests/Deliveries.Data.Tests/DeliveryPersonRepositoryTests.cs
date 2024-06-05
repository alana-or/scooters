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
public class DeliveryPersonRepositoryTests
{
    private PostgreSqlContainer _postgresContainer;
    private ServiceProvider _serviceProvider;
    private Mock<ILogger<DeliveryPeopleRepository>> _loggerMock;

    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        _postgresContainer = new PostgreSqlBuilder()
            .WithDatabase("scooters_db")
            .WithUsername("postgres")
            .WithPassword("postgrespw")
            .WithCleanUp(true)
            .Build();

        _loggerMock = new Mock<ILogger<DeliveryPeopleRepository>>();
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
        services.AddScoped<IDeliveryPersonRepository, DeliveryPeopleRepository>();

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
    public async Task Create_Should_Create_DeliveryPerson()
    {
        using (var _scope = _serviceProvider.CreateScope())
        {
            var _deliveries = _scope.ServiceProvider.GetRequiredService<IDeliveryPersonRepository>();
            var _context = _scope.ServiceProvider.GetRequiredService<DeliveriesContext>();

            var delivery = new DeliveryPersonBuilder().Build();

            await _deliveries.CreateAsync(delivery);

            var updatedDelivery = await _context.DeliveryPeople.FindAsync(delivery.Id);

            updatedDelivery.Name.Should().Be(delivery.Name);
            updatedDelivery.CNHImage.Should().Be(delivery.CNHImage);
        }
    }

    [Test]
    public async Task Create_Should_Log_Error_When_Exception_Occurs()
    {
        using (var _scope = _serviceProvider.CreateScope())
        {
            var _deliveries = _scope.ServiceProvider.GetRequiredService<IDeliveryPersonRepository>();

            DisposeDataBase(_scope);

            Func<Task> act = async () => await _deliveries.CreateAsync(new DeliveryPersonDb());

            await act.Should().ThrowAsync<Exception>();
            VerifyThrowExeption("An error occurred while creating delivery person.", LogLevel.Error, Times.Once());
        }
    }

    [Test]
    public async Task Update_Should_Update_DeliveryPerson()
    {
        using (var _scope = _serviceProvider.CreateScope())
        {
            var _deliveries = _scope.ServiceProvider.GetRequiredService<IDeliveryPersonRepository>();
            var _context = _scope.ServiceProvider.GetRequiredService<DeliveriesContext>();
           
            var deliveryPerson = new DeliveryPersonBuilder().Build();

            _context.DeliveryPeople.Add(deliveryPerson);
            await _context.SaveChangesAsync();

            deliveryPerson.Name = "New Name";
            deliveryPerson.CNHImage = "New Photo";
            await _deliveries.UpdateAsync(deliveryPerson);

            var updatedDeliveryPerson = await _context.DeliveryPeople.FindAsync(deliveryPerson.Id);

            updatedDeliveryPerson.Name.Should().Be("New Name");
            updatedDeliveryPerson.CNHImage.Should().Be("New Photo");
        }
    }

    [Test]
    public async Task Update_Should_Log_Error_When_Exception_Occurs()
    {
        using (var _scope = _serviceProvider.CreateScope())
        {
            var _deliveries = _scope.ServiceProvider.GetRequiredService<IDeliveryPersonRepository>();

            DisposeDataBase(_scope);

            Func<Task> act = async () => await _deliveries.UpdateAsync(new DeliveryPersonDb());

            await act.Should().ThrowAsync<Exception>();
            VerifyThrowExeption("An error occurred while updating delivery person.", LogLevel.Error, Times.Once());
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