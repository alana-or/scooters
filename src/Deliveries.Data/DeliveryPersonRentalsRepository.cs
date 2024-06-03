using Bogus;
using Deliveries.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Deliveries.Data;

public interface IDeliveryPersonRentalsRepository
{
    public Task CreateAsync(DeliveryPersonRentalDb deliveryRentalDb);
    public Task UpdateAsync(DeliveryPersonRentalDb deliveryRentalDb);
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
            _context.DeliveryPersonRentals.Add(personRental);
            await _context.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating delivery rental.");
            throw;
        }
    }

    public Task UpdateAsync(DeliveryPersonRentalDb deliveryRentalDb)
    {
        throw new NotImplementedException();
    }
}
