using Deliveries.Api.Services;
using Deliveries.Api.Validations;
using Deliveries.Data;
using FluentValidation;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Deliveries.Api;

public class Startup(IConfiguration configuration)
{
    public IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<DbContext, DeliveriesContext>();

        services.AddDbContext<DeliveriesContext>(options =>
            options.UseNpgsql(
                Configuration.GetConnectionString("DefaultConnection")).EnableDetailedErrors());

        services.AddScoped<IDeliveryPersonRentalsRepository, DeliveryPersonRentalsRepository>();
        services.AddScoped<IDeliveryPersonRepository, DeliveryPersonRepository>();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddControllers();

        services.AddScoped<IDeliveriesService, DeliveriesService>();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddValidatorsFromAssemblyContaining<DeliveryPersonCreateValidator>();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo()
            {
                Version = "v1",
                Title = "Deliveries Api",
                Description = "O sistema é.",
            });
        });
    }

    public void Configure(WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        ConfigureRedirectToSwagger(app);

        app.UseAuthorization();

        app.MapControllers();
    }

    private static void ConfigureRedirectToSwagger(WebApplication app)
    {
        var rewriterOptions = new RewriteOptions().AddRedirect("^$", "swagger");
        app.UseRewriter(rewriterOptions);
    }
}
