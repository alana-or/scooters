using Microsoft.Extensions.DependencyInjection;

namespace Deliveries.Api.Tests.Integration;

public class BaseTest
{
    public HttpClient Client { get; set; }

    public IServiceProvider ServiceProvider { get; set; }

    private IServiceScope scope;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        ServiceProvider = scope.ServiceProvider;
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        scope.Dispose();
    }
}
