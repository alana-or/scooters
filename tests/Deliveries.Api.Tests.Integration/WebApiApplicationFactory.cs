using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Deliveries.Api.Tests.Integration;

public class WebApiApplicationFactory : WebApplicationFactory<Startup>
{

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(config =>
        {
            var integrationAppSettings = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Tests.json")
                .Build();

            config.AddConfiguration(integrationAppSettings);
        });

        return base.CreateHost(builder);
    }

}