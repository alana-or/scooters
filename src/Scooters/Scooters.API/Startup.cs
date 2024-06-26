﻿using FluentValidation;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Scooters.Api.Services;
using Scooters.Api.Validations;
using Scooters.Data;

namespace Soooters.Api;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
        => Configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<DbContext, ScootersContext>();

        services.AddDbContext<ScootersContext>(options =>
            options.UseNpgsql(
                Configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IScootersRepository, ScootersRepository>();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddControllers();

        services.AddScoped<IScootersService, ScooterService>();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddValidatorsFromAssemblyContaining<ScooterCreateValidator>();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo()
            {
                Version = "v1",
                Title = "Scooters API",
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
