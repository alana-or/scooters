using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Testcontainers.PostgreSql;

[SetUpFixture]
public class PostgreSqlTestcontainerFixture
{
    public PostgreSqlContainer Testcontainer { get; private set; }
    public string ConnectionString { get; private set; }

    [OneTimeSetUp]
    public async Task InitializeAsync()
    {
        Testcontainer = new PostgreSqlBuilder()
            .WithHostname("deliveries_db")
            .WithDatabase("deliveries_db")
            .WithUsername("postgres")
            .WithPassword("postgrespw")
            .WithCleanUp(true)
            .Build();
        
        await Testcontainer.StartAsync();

        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Tests.json")
            .Build();

        ConnectionString = builder.GetConnectionString("DefaultConnection");
    }

    [OneTimeTearDown]
    public async Task DisposeAsync()
    {
        await Testcontainer.StopAsync();
        await Testcontainer.DisposeAsync();
    }
}
