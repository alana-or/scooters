using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;

[SetUpFixture]
public class PostgreSqlTestcontainerFixture
{
    public PostgreSqlContainer Testcontainer { get; private set; }
    public string ConnectionString { get; private set; }

    [OneTimeSetUp]
    public async Task InitializeAsync()
    {
        // Set up PostgreSQL container
        Testcontainer = new PostgreSqlBuilder()
            .WithDatabase("deliveries_db")
            .WithUsername("postgres")
            .WithPassword("postgrespw")
            .WithPortBinding(64941, 5432)
            .WithCleanUp(true)
            .Build();

        await Testcontainer.StartAsync();
        ConnectionString = Testcontainer.GetConnectionString();
    }

    [OneTimeTearDown]
    public async Task DisposeAsync()
    {
        await Testcontainer.StopAsync();
        await Testcontainer.DisposeAsync();
    }
}

