using AutoBogus;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Motos.Data;
using Motos.Data.Builders;
using Motos.Data.Entities;
using Testcontainers.PostgreSql;


[TestFixture]
public class MotoRepositoryTests
{
    private PostgreSqlContainer _postgresContainer;
    private ServiceProvider _serviceProvider;
    private Mock<ILogger<MotosRepository>> _loggerMock;

    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        _postgresContainer = new PostgreSqlBuilder()
            .WithDatabase("motos_db")
            .WithUsername("postgres")
            .WithPassword("postgrespw")
            .WithCleanUp(true)
            .Build();

        _loggerMock = new Mock<ILogger<MotosRepository>>();
        _loggerMock.SetupAllProperties();

        await _postgresContainer.StartAsync();

        var services = new ServiceCollection();

        services.AddSingleton(_loggerMock.Object);

        services.AddLogging(config =>
        {
            config.SetMinimumLevel(LogLevel.Information);
            config.AddConsole(); 
        });

        services.AddDbContext<MotosContext>(options =>
            options.UseNpgsql(_postgresContainer.GetConnectionString()));
        services.AddScoped<IMotosRepository, MotosRepository>();

        _serviceProvider = services.BuildServiceProvider();

        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<MotosContext>();
            context.Database.Migrate();
        }
    }


    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _postgresContainer.DisposeAsync();
        await _serviceProvider.DisposeAsync();
    }


    [Test]
    public async Task GetMotoAsync_Should_Log_Error_When_Exception_Occurs()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var _motosRepository = scope.ServiceProvider.GetRequiredService<IMotosRepository>();
            var _context = scope.ServiceProvider.GetRequiredService<MotosContext>();

            var moto = new MotoBuilder().Build();
            _context.Dispose();

            Func<Task> act = async () => await _motosRepository.GetMotoAsync(null);

            await act.Should().ThrowAsync<Exception>();

            await Task.Delay(100);

            _loggerMock.Verify(l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("An error occurred while getting motos.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }
    }
}