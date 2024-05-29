using Bogus;
using Deliveries.Data.Entities;
using Microsoft.EntityFrameworkCore;

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

    public async Task<DeliveryPersonDb> GetDeliveryPersonAsync(Guid personId)
    {
        try
        {
            return await _context
                .DeliveryPeople
                .FirstAsync(x => x.Id == personId);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while getting the delivery person.");
            throw;
        }
    }

    public Task<IEnumerable<DeliveryPersonDb>> GetDeliveryPersonAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<DeliveryPersonRentalDb>> GetRentals(Guid personId)
    {
        try
        {
            return await _context
                .DeliveryPersonRentals
                .Where(x => x.DeliveryPerson.Id == personId)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while getting rentals.");
            throw;
        }
    }

    public async Task CreateDeliveryRentalAsync(DeliveryPersonRentalDb deliveryRentalDb)
    {
        try
        {
            _context.DeliveryPersonRentals.Add(deliveryRentalDb);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while creating delivery rental.");
            throw;
        }
    }
}
