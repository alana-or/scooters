using Microsoft.EntityFrameworkCore;
using Deliveries.Data.Entities;

namespace Deliveries.Data;

public class DeliveriesRepository(DeliveriesContext context) : IDeliveriesRepository
{
    private readonly DeliveriesContext _context = context;
    private readonly Serilog.ILogger _logger;

    public async Task CreateAsync(DeliveryPersonDb person)
    {
        try
        {
            _context.DeliveryPeople.Add(person);
            await _context.SaveChangesAsync();
        }
        catch(Exception ex) 
        {
            _logger.Error(ex, "An error occurred while creating delivery person.");
            throw;
        }
    }

    public async Task UpdateAsync(DeliveryPersonDb person)
    {
        try
        {
            _context.DeliveryPeople.Update(person);
            await _context.SaveChangesAsync();
        }
        catch(Exception ex) 
        {
            _logger.Error(ex, "An error occurred while updating delivery person.");
            throw;
        }
    }

    public async Task<IEnumerable<DeliveryPersonDb>> GetDeliveryPeopleAsync()
    {
        try
        {
            return await _context.DeliveryPeople
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while getting delivery persons.");
            throw;
        }
    }
}
