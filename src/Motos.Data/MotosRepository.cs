using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Motto.Entities;

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

    public async Task CreateMoto(Moto moto)
    {
        try
        {
            context.motos.Add(moto);
            await context.SaveChangesAsync();
        }
        catch(Exception ex) 
        {
            logger.LogError(ex, "An error occurred while getting motos.");
            throw;
        }
    }

    public async Task UpdateMoto(Moto moto)
    {
        try
        {
            context.motos.Update(moto);
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
                return await context.motos.ToListAsync();
            }

            return await context.motos
                .Where(x => x.placa == placa)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting motos.");
            throw;
        }
    }
}
