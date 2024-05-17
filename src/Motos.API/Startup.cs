using FluentValidation;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Motos.Application.Create.Validations;
using Motos.Data;
using System;

namespace Motos;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
        => Configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddScoped<CreateMotoUseCase>();
        services.AddScoped<SelectMotoUseCase>();
        services.AddScoped<UpdateMotoUseCase>();

        services.AddScoped<MotosContext>();

        services.AddDbContext<MotosContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IMotosRepository, MotosRepository>();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddValidatorsFromAssemblyContaining<CreateMotoValidator>();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo()
            {
                Version = "v1",
                Title = "Samples API",
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