using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Motos.Data.Builders;
using Motos.Data.Entities;
using Testcontainers.PostgreSql;

namespace Motos.Data.Tests;

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
    public async Task Create_Should_Create_Moto()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var motosRepository = scope.ServiceProvider.GetRequiredService<IMotosRepository>();
            var context = scope.ServiceProvider.GetRequiredService<MotosContext>();

            var moto = new MotoDB { Id = 101, Ano = 2023, Modelo = "Model", Placa = "123" };

            await motosRepository.Create(moto);

            var updatedMoto = await context.Motos.FindAsync(moto.Id);

            updatedMoto.Ano.Should().Be(2023);
            updatedMoto.Modelo.Should().Be("Model");
            updatedMoto.Placa.Should().Be("123");
        }
    }

    [Test]
    public async Task Create_Should_Log_Error_When_Exception_Occurs()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var motosRepository = scope.ServiceProvider.GetRequiredService<IMotosRepository>();

            DisposeDataBase(scope);

            Func<Task> act = async () => await motosRepository.Create(new MotoDB());

            await act.Should().ThrowAsync<Exception>();
            VerifyThrowExeption("An error occurred while creating moto.", LogLevel.Error, Times.Once());
        }
    }

    [Test]
    public async Task Update_Should_Update_Moto()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var motosRepository = scope.ServiceProvider.GetRequiredService<IMotosRepository>();
            var context = scope.ServiceProvider.GetRequiredService<MotosContext>();

            var moto = new MotoDB { Id = 101, Ano = 2023, Modelo = "Old Model", Placa = "OLD123" };
            context.Motos.Add(moto);
            await context.SaveChangesAsync();

            moto.Ano = 2024;
            moto.Modelo = "New Model";
            moto.Placa = "NEW123";
            await motosRepository.Update(moto);

            var updatedMoto = await context.Motos.FindAsync(moto.Id);

            updatedMoto.Ano.Should().Be(2024);
            updatedMoto.Modelo.Should().Be("New Model");
            updatedMoto.Placa.Should().Be("NEW123");
        }
    }

    [Test]
    public async Task Update_Should_Log_Error_When_Exception_Occurs()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var motosRepository = scope.ServiceProvider.GetRequiredService<IMotosRepository>();

            DisposeDataBase(scope);

            Func<Task> act = async () => await motosRepository.Update(new MotoDB());

            await act.Should().ThrowAsync<Exception>();
            VerifyThrowExeption("An error occurred while updating moto.", LogLevel.Error, Times.Once());
        }
    }

    [Test]
    public async Task GetMotoAsync_Should_Return_All_Motos_When_Placa_Is_Null_Or_Empty()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var motosRepository = scope.ServiceProvider.GetRequiredService<IMotosRepository>();
            var context = scope.ServiceProvider.GetRequiredService<MotosContext>();
            
            CleanDataBase(context);

            var motos = new MotoBuilder().Generate(5);
            context.Motos.AddRange(motos);
            await context.SaveChangesAsync();

            var result = await motosRepository.GetMotoAsync(null);

            result.Should().HaveCount(5);
        }
    }

    [Test]
    public async Task GetMotoAsync_Should_Return_Specific_Moto_When_Placa_Is_Provided()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var motosRepository = scope.ServiceProvider.GetRequiredService<IMotosRepository>();
            var context = scope.ServiceProvider.GetRequiredService<MotosContext>();

            var motos = new MotoBuilder().Generate(5);

            var specificMoto = new MotoDB { Ano = 2023, Modelo = "Specific Model", Placa = "ABC123" };
            motos.Add(specificMoto);

            context.Motos.AddRange(motos);
            await context.SaveChangesAsync();

            var result = await motosRepository.GetMotoAsync("ABC123");

            result.Should().HaveCount(1);
            result.First().Modelo.Should().Be("Specific Model");
            result.First().Placa.Should().Be("ABC123");
        }
    }

    [Test]
    public async Task GetMotoAsync_Should_Log_Error_When_Exception_Occurs()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var motosRepository = scope.ServiceProvider.GetRequiredService<IMotosRepository>();
            
            DisposeDataBase(scope);

            Func<Task> act = async () => await motosRepository.GetMotoAsync(null);

            await act.Should().ThrowAsync<Exception>();
            VerifyThrowExeption("An error occurred while getting motos.", LogLevel.Error, Times.Once());
        }
    }

    [Test]
    public async Task SaveMoto2024Async_Should_Create_Motos_Log()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var motosRepository = scope.ServiceProvider.GetRequiredService<IMotosRepository>();
            var context = scope.ServiceProvider.GetRequiredService<MotosContext>();

            CleanDataBase(context);

            var motosLog = new MotoLog2024Builder().Generate(5);
            context.MotosLog2024.AddRange(motosLog);

            await context.SaveChangesAsync();

            var motoLog = new MotosLog2024("message"); 

            await motosRepository.SaveMoto2024Async(motoLog);

            var motoLogDB = await context.MotosLog2024
                .FirstOrDefaultAsync(x => x.Message == motoLog.Message);

            motoLogDB.Message.Should().Be("message");
        }
    }

    [Test]
    public async Task SaveMoto2024Async_Should_Log_Error_When_Exception_Occurs()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var motosRepository = scope.ServiceProvider.GetRequiredService<IMotosRepository>();

            DisposeDataBase(scope);

            Func<Task> act = async () => await motosRepository.SaveMoto2024Async(new MotosLog2024(""));

            await act.Should().ThrowAsync<Exception>();
            VerifyThrowExeption("An error occurred while creating moto log.", LogLevel.Error, Times.Once());
        }
    }

    [Test]
    public async Task GetMoto2024Async_Should_Return_All_Motos_When_Placa_Is_Null_Or_Empty()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var motosRepository = scope.ServiceProvider.GetRequiredService<IMotosRepository>();
            var context = scope.ServiceProvider.GetRequiredService<MotosContext>();

            CleanDataBase(context);

            var motosLog = new MotoLog2024Builder().Generate(5);

            context.MotosLog2024.AddRange(motosLog);
            await context.SaveChangesAsync();

            var result = await motosRepository.GetMoto2024Async();

            result.Should().HaveCount(5);
        }
    }

    [Test]
    public async Task GetMoto2024Async_Should_Log_Error_When_Exception_Occurs()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var motosRepository = scope.ServiceProvider.GetRequiredService<IMotosRepository>();

            DisposeDataBase(scope);

            Func<Task> act = async () => await motosRepository.GetMoto2024Async();

            await act.Should().ThrowAsync<Exception>();
            VerifyThrowExeption("An error occurred while getting motos log.", LogLevel.Error, Times.Once());
        }
    }

    private void CleanDataBase(MotosContext context)
    {
        context.Motos.RemoveRange(context.Motos);
        context.MotosLog2024.RemoveRange(context.MotosLog2024);
        context.SaveChanges();
    }

    private static void DisposeDataBase(IServiceScope scope)
    {
        var context = scope.ServiceProvider.GetRequiredService<MotosContext>();
        context.Dispose();
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