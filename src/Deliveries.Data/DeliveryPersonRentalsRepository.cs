using Bogus;
using Deliveries.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Deliveries.Data;

public interface IDeliveryPersonRentalsRepository
{
    public Task CreateAsync(DeliveryPersonRentalDb deliveryRentalDb);
    public Task UpdateAsync(DeliveryPersonRentalDb deliveryRentalDb);
    public Task<IEnumerable<DeliveryPersonRentalDb>> GetDeliveryPersonRentalsAsync(Guid id);
}

public class DeliveryPersonRentalsRepository : IDeliveryPersonRentalsRepository
{
    private readonly DeliveriesContext _context;
    private readonly ILogger<DeliveryPersonRentalsRepository> _logger;

    public DeliveryPersonRentalsRepository(DeliveriesContext context, ILogger<DeliveryPersonRentalsRepository> logger) 
    {
        _logger = logger;
        _context = context;
    }

    public async Task CreateAsync(DeliveryPersonRentalDb personRental)
    {
        try
        {
            personRental.DeliveryPerson = null;
            _context.DeliveryPersonRentals.Add(personRental);
            await _context.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating delivery rental.");
            throw;
        }
    }

    public async Task UpdateAsync(DeliveryPersonRentalDb deliveryRental)
    {
        try
        {
            _context.DeliveryPersonRentals.Update(deliveryRental);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating delivery rental.");
            throw;
        }
    }

    public async Task<IEnumerable<DeliveryPersonRentalDb>> GetDeliveryPersonRentalsAsync(Guid id)
    {
        try
        {
            return await _context.DeliveryPersonRentals
                .Include(x => x.DeliveryPerson)
                .Where(x => x.DeliveryPerson.Id == id)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting delivery person rentals.");
            throw;
        }
    }
}
