using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Scooters.Data.Entities;

namespace Scooters.Data;

public class ScootersRepository : IScootersRepository
{
    private readonly ScootersContext context;
    private readonly ILogger<ScootersRepository> logger;

    public ScootersRepository(ScootersContext context, ILogger<ScootersRepository> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task CreateAsync(ScooterDB moto)
    {
        try
        {
            context.Scooters.Add(moto);
            await context.SaveChangesAsync();
        }
        catch(Exception ex) 
        {
            logger.LogError(ex, "An error occurred while creating scooter.");
            throw;
        }
    }

    public async Task UpdateAsync(ScooterDB moto)
    {
        try
        {
            context.Scooters.Update(moto);
            await context.SaveChangesAsync();
        }
        catch(Exception ex) 
        {
            logger.LogError(ex, "An error occurred while updating scooter.");
            throw;
        }
    }

    public async Task<IEnumerable<ScooterDB>> GetScootersAsync(string? licence_plate)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(licence_plate))
            {
                return await context.Scooters.ToListAsync();
            }

            return await context.Scooters
                .Where(x => x.LicencePlate == licence_plate)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting scooters.");
            throw;
        }
    }

    public async Task<IEnumerable<ScooterLog2024>> GetScooters2024Async()
    {
        try
        {
            return await context.ScootersLog2024
               .ToListAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting scooters log.");
            throw;
        }
    }

    public async Task SaveScooter2024Async(ScooterLog2024 message)
    {
        try
        {
            context.ScootersLog2024.Add(message);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while creating scooter log.");
            throw;
        }
    }
}
