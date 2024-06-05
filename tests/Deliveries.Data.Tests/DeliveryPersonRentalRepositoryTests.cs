using AutoMapper;
using Deliveries.Application;
using Deliveries.Application.Mappers;
using Deliveries.Data.Builders;
using Deliveries.Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Testcontainers.PostgreSql;

namespace Deliveries.Data.Tests.Integration;

[TestFixture]
public class DeliveryPersonRentalRepositoryTests
{
    private PostgreSqlContainer _postgresContainer;
    private ServiceProvider _serviceProvider;
    private Mock<ILogger<DeliveryPersonRentalsRepository>> _loggerMock;
    private IMapper _mapper;

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
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<DeliveriesMapper>();
        });

        _mapper = mapperConfig.CreateMapper();

        services.AddSingleton<IMapper>(_mapper);
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
            var personDb = new DeliveryPersonDbBuilder().Build();
            _context.DeliveryPeople.Add(personDb);
            await _context.SaveChangesAsync();

            var deliveryPerson = _mapper.Map<DeliveryPerson>(personDb);

            var delivery = new DeliveryPersonRentalBuilder()
                .WithPerson(deliveryPerson)
                .WithYear(2023)
                .WithModel("Model")
                .WithLicencePlace("LicencePlate")
                .Build();

            await _deliveries.CreateAsync(delivery);

            var updatedDeliveryRental = await _context.DeliveryPersonRentals
                .FirstAsync(x => x.DeliveryPerson.Id == personDb.Id);

            updatedDeliveryRental.Model.Should().Be("Model");
            updatedDeliveryRental.Year.Should().Be(2023);
            updatedDeliveryRental.LicencePlate.Should().Be("LicencePlate");
            updatedDeliveryRental.DeliveryPerson.Name.Should().Be(personDb.Name);
            updatedDeliveryRental.DeliveryPerson.CNHImage.Should().Be(personDb.CNHImage);
        }
    }

    [Test]
    public async Task Create_Should_Log_Error_When_Exception_Occurs()
    {
        using (var _scope = _serviceProvider.CreateScope())
        {
            var _deliveries = _scope.ServiceProvider.GetRequiredService<IDeliveryPersonRentalsRepository>();

            DisposeDataBase(_scope);

            Func<Task> act = async () => await _deliveries.CreateAsync(new DeliveryPersonRentalBuilder().Build());

            await act.Should().ThrowAsync<Exception>();
            VerifyThrowExeption("An error occurred while creating delivery rental.", LogLevel.Error, Times.Once());
        }
    }

    [Test]
    public async Task Update_Should_Update_DeliveryPersonRental()
    {
        var deliveryRentalDb = new DeliveryPersonRentalDbBuilder().Build(); 

        using (var _scope = _serviceProvider.CreateScope())
        {
            var _deliveries = _scope.ServiceProvider.GetRequiredService<IDeliveryPersonRentalsRepository>();
            var _context = _scope.ServiceProvider.GetRequiredService<DeliveriesContext>();

            _context.DeliveryPersonRentals.Add(deliveryRentalDb);
            await _context.SaveChangesAsync();
        }

        using (var _scope = _serviceProvider.CreateScope())
        { 
            var _deliveries = _scope.ServiceProvider.GetRequiredService<IDeliveryPersonRentalsRepository>();
            var _context = _scope.ServiceProvider.GetRequiredService<DeliveriesContext>();

            deliveryRentalDb.LicencePlate = "New LicencePlate";
            deliveryRentalDb.Model = "New Model";
            
            var deliveryPersonRental = _mapper.Map<DeliveryPersonRental>(deliveryRentalDb);

            await _deliveries.UpdateAsync(deliveryPersonRental);

            var updatedDeliveryPerson = await _context
                .DeliveryPersonRentals
                .FindAsync(deliveryRentalDb.Id);

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

            Func<Task> act = async () => await _deliveries.UpdateAsync(new DeliveryPersonRentalBuilder().Build());

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