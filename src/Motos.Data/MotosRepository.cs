using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Motos.Data.Entities;

namespace Motos.Data;

public class MotosRepository : IMotosRepository
{
    private readonly MotosContext context;
    private readonly ILogger<MotosRepository> logger;

    public MotosRepository(MotosContext context, ILogger<MotosRepository> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task Create(Moto moto)
    {
        try
        {
            context.Motos.Add(moto);
            await context.SaveChangesAsync();
        }
        catch(Exception ex) 
        {
            logger.LogError(ex, "An error occurred while getting motos.");
            throw;
        }
    }

    public async Task Update(Moto moto)
    {
        try
        {
            context.Motos.Update(moto);
            await context.SaveChangesAsync();
        }
        catch(Exception ex) 
        {
            logger.LogError(ex, "An error occurred while getting motos.");
            throw;
        }
    }

    public async Task<IEnumerable<Moto>> GetMotoAsync(string? placa)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(placa))
            {
                return await context.Motos.ToListAsync();
            }

            return await context.Motos
                .Where(x => x.Placa == placa)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting motos.");
            throw;
        }
    }

    public async Task<IEnumerable<MotosLog2024>> GetMoto2024Async()
    {
        try
        {
            return await context.MotosLog2024
               .OrderByDescending(x => x.Id) 
               .Take(5)
               .ToListAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting motos.");
            throw;
        }
    }

    public async Task SaveMoto2024Async(MotosLog2024 message)
    {
        try
        {
            context.MotosLog2024.Add(message);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while creating motos log.");
            throw;
        }
    }
}
