using Deliveries.Api;
using Deliveries.Api.Services;
using Deliveries.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

[SetUpFixture]
public class ApiFixture
{
    private PostgreSqlContainer _testcontainer;
    private readonly WebApplicationFactory<Startup> _factory = new WebApplicationFactory<Startup>();
    public static HttpClient HttpClient { get; private set; }

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _testcontainer = new PostgreSqlBuilder()
            .WithDatabase("deliveries_db")
            .WithUsername("postgres")
            .WithPassword("postgrespw")
            .WithPortBinding(64941, 5432)
            .WithCleanUp(true)
            .Build();

        await _testcontainer.StartAsync();

        HttpClient = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddScoped<DbContext, DeliveriesContext>();

                services.AddDbContext<DeliveriesContext>(options =>
                    options.UseNpgsql(_testcontainer.GetConnectionString()).EnableDetailedErrors());

                services.AddScoped<IDeliveryPersonRentalsRepository, DeliveryPersonRentalsRepository>();
                services.AddScoped<IDeliveryPersonRepository, DeliveryPersonRepository>();
                services.AddScoped<IDeliveriesService, DeliveriesService>();

            });
        }).CreateClient();

        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DeliveriesContext>();
        dbContext.Database.Migrate();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        HttpClient?.Dispose();
        _factory.Dispose();
        await _testcontainer.StopAsync();
        await _testcontainer.DisposeAsync();
    }
}

