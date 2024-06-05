using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Scooters.Data.Builders;
using Scooters.Data.Entities;
using Testcontainers.PostgreSql;

namespace Scooters.Data.Tests.Integration;

[TestFixture]
public class ScootersRepositoryTests
{
    private PostgreSqlContainer _postgresContainer;
    private ServiceProvider _serviceProvider;
    private Mock<ILogger<ScootersRepository>> _loggerMock;

    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        _postgresContainer = new PostgreSqlBuilder()
            .WithDatabase("scooters_db")
            .WithUsername("postgres")
            .WithPassword("postgrespw")
            .WithCleanUp(true)
            .Build();

        _loggerMock = new Mock<ILogger<ScootersRepository>>();
        _loggerMock.SetupAllProperties();

        await _postgresContainer.StartAsync();

        var services = new ServiceCollection();

        services.AddSingleton(_loggerMock.Object);

        services.AddLogging(config =>
        {
            config.SetMinimumLevel(LogLevel.Information);
            config.AddConsole(); 
        });

        services.AddDbContext<ScootersContext>(options =>
            options.UseNpgsql(_postgresContainer.GetConnectionString()));
        services.AddScoped<IScootersRepository, ScootersRepository>();

        _serviceProvider = services.BuildServiceProvider();

        using (var _scope = _serviceProvider.CreateScope())
        {
            var _context = _scope.ServiceProvider.GetRequiredService<ScootersContext>();
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
    public async Task Create_Should_Create_Scooter()
    {
        using (var _scope = _serviceProvider.CreateScope())
        {
            var _scooters = _scope.ServiceProvider.GetRequiredService<IScootersRepository>();
            var _context = _scope.ServiceProvider.GetRequiredService<ScootersContext>();

            var scooter = new ScooterDB { Id = 10, Year = 2023, Model = "Model", LicencePlate = "123" };

            await _scooters.CreateAsync(scooter);

            var updatedScooter = await _context.Scooters.FindAsync(scooter.Id);

            updatedScooter.Year.Should().Be(2023);
            updatedScooter.Model.Should().Be("Model");
            updatedScooter.LicencePlate.Should().Be("123");
        }
    }

    [Test]
    public async Task Create_Should_Log_Error_When_Exception_Occurs()
    {
        using (var _scope = _serviceProvider.CreateScope())
        {
            var _scooters = _scope.ServiceProvider.GetRequiredService<IScootersRepository>();

            DisposeDataBase(_scope);

            Func<Task> act = async () => await _scooters.CreateAsync(new ScooterDB());

            await act.Should().ThrowAsync<Exception>();
            VerifyThrowExeption("An error occurred while creating scooter.", LogLevel.Error, Times.Once());
        }
    }

    [Test]
    public async Task Update_Should_Update_Scooter()
    {
        using (var _scope = _serviceProvider.CreateScope())
        {
            var _scooters = _scope.ServiceProvider.GetRequiredService<IScootersRepository>();
            var _context = _scope.ServiceProvider.GetRequiredService<ScootersContext>();

            var scooter = new ScooterDB { Id = 10, Year = 2023, Model = "Old Model", LicencePlate = "OLD123" };
            _context.Scooters.Add(scooter);
            await _context.SaveChangesAsync();

            scooter.Year = 2024;
            scooter.Model = "New Model";
            scooter.LicencePlate = "NEW123";
            await _scooters.UpdateAsync(scooter);

            var updatedScooter = await _context.Scooters.FindAsync(scooter.Id);

            updatedScooter.Year.Should().Be(2024);
            updatedScooter.Model.Should().Be("New Model");
            updatedScooter.LicencePlate.Should().Be("NEW123");
        }
    }

    [Test]
    public async Task Update_Should_Log_Error_When_Exception_Occurs()
    {
        using (var _scope = _serviceProvider.CreateScope())
        {
            var _scooters = _scope.ServiceProvider.GetRequiredService<IScootersRepository>();

            DisposeDataBase(_scope);

            Func<Task> act = async () => await _scooters.UpdateAsync(new ScooterDB());

            await act.Should().ThrowAsync<Exception>();
            VerifyThrowExeption("An error occurred while updating scooter.", LogLevel.Error, Times.Once());
        }
    }

    [Test]
    public async Task GetScooterAsync_Should_Return_All_Scooters_When_LicencePlate_Is_Null_Or_Empty()
    {
        using (var _scope = _serviceProvider.CreateScope())
        {
            var _scooters = _scope.ServiceProvider.GetRequiredService<IScootersRepository>();
            var _context = _scope.ServiceProvider.GetRequiredService<ScootersContext>();
            
            CleanDataBase(_context);

            var scooters = new ScooterBuilder().Generate(5);
            _context.Scooters.AddRange(scooters);
            await _context.SaveChangesAsync();

            var result = await _scooters.GetScootersAsync(null);

            result.Should().HaveCount(5);
        }
    }

    [Test]
    public async Task GetScooterAsync_Should_Return_Specific_Scooter_When_LicencePlate_Is_Provided()
    {
        using (var _scope = _serviceProvider.CreateScope())
        {
            var _scooters = _scope.ServiceProvider.GetRequiredService<IScootersRepository>();
            var _context = _scope.ServiceProvider.GetRequiredService<ScootersContext>();

            var scooters = new ScooterBuilder().Generate(5);

            var specificScooter = new ScooterBuilder()
                .SetLicencePlate("ABC123")
                .SetModel("Specific Model")
                .Build();

            scooters.Add(specificScooter);

            _context.Scooters.AddRange(scooters);
            await _context.SaveChangesAsync();

            var result = await _scooters.GetScootersAsync("ABC123");

            result.Should().HaveCount(1);
            result.First().Model.Should().Be("Specific Model");
            result.First().LicencePlate.Should().Be("ABC123");
        }
    }

    [Test]
    public async Task GetScooterAsync_Should_Log_Error_When_Exception_Occurs()
    {
        using (var _scope = _serviceProvider.CreateScope())
        {
            var _scooters = _scope.ServiceProvider.GetRequiredService<IScootersRepository>();
            
            DisposeDataBase(_scope);

            Func<Task> act = async () => await _scooters.GetScootersAsync(null);

            await act.Should().ThrowAsync<Exception>();
            VerifyThrowExeption("An error occurred while getting scooters.", LogLevel.Error, Times.Once());
        }
    }

    [Test]
    public async Task SaveScooter2024Async_Should_Create_Scooters_Log()
    {
        using (var _scope = _serviceProvider.CreateScope())
        {
            var _scooters = _scope.ServiceProvider.GetRequiredService<IScootersRepository>();
            var _context = _scope.ServiceProvider.GetRequiredService<ScootersContext>();

            CleanDataBase(_context);

            var scootersLog = new ScooterLog2024Builder().Generate(5);
            _context.ScootersLog2024.AddRange(scootersLog);

            await _context.SaveChangesAsync();

            var scooterLog = new ScooterLog2024("message"); 

            await _scooters.SaveScooter2024Async(scooterLog);

            var scooterLogDB = await _context.ScootersLog2024
                .FirstOrDefaultAsync(x => x.Message == scooterLog.Message);

            scooterLogDB.Message.Should().Be("message");
        }
    }

    [Test]
    public async Task SaveScooter2024Async_Should_Log_Error_When_Exception_Occurs()
    {
        using (var _scope = _serviceProvider.CreateScope())
        {
            var _scooters = _scope.ServiceProvider.GetRequiredService<IScootersRepository>();

            DisposeDataBase(_scope);

            Func<Task> act = async () => await _scooters.SaveScooter2024Async(new ScooterLog2024(""));

            await act.Should().ThrowAsync<Exception>();
            VerifyThrowExeption("An error occurred while creating scooter log.", LogLevel.Error, Times.Once());
        }
    }

    [Test]
    public async Task GetScooter2024Async_Should_Return_All_Scooters_When_LicencePlate_Is_Null_Or_Empty()
    {
        using (var _scope = _serviceProvider.CreateScope())
        {
            var _scooters = _scope.ServiceProvider.GetRequiredService<IScootersRepository>();
            var _context = _scope.ServiceProvider.GetRequiredService<ScootersContext>();

            CleanDataBase(_context);

            var scootersLog = new ScooterLog2024Builder().Generate(5);

            _context.ScootersLog2024.AddRange(scootersLog);
            await _context.SaveChangesAsync();

            var result = await _scooters.GetScooters2024Async();

            result.Should().HaveCount(5);
        }
    }

    [Test]
    public async Task GetScooter2024Async_Should_Log_Error_When_Exception_Occurs()
    {
        using (var _scope = _serviceProvider.CreateScope())
        {
            var _scooters = _scope.ServiceProvider.GetRequiredService<IScootersRepository>();

            DisposeDataBase(_scope);

            Func<Task> act = _scooters.GetScooters2024Async;

            await act.Should().ThrowAsync<Exception>();
            VerifyThrowExeption("An error occurred while getting scooters log.", LogLevel.Error, Times.Once());
        }
    }

    private void CleanDataBase(ScootersContext _context)
    {
        _context.Scooters.RemoveRange(_context.Scooters);
        _context.ScootersLog2024.RemoveRange(_context.ScootersLog2024);
        _context.SaveChanges();
    }

    private static void DisposeDataBase(IServiceScope _scope)
    {
        var _context = _scope.ServiceProvider.GetRequiredService<ScootersContext>();
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