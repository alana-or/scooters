using Microsoft.EntityFrameworkCore;
using Motos.Data;

namespace Motos.Consumer;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
        => Configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<DbContext, MotosContext>();

        services.AddDbContext<MotosContext>(options =>
            options.UseNpgsql(
                Configuration.GetConnectionString("DefaultConnection")).EnableDetailedErrors());

        services.AddHostedService<RabbitMQBackgroundService>();

        services.AddScoped<IMotosRepository, MotosRepository>();
    }
}
