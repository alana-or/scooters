using AutoMapper;
using Deliveries.Application;
using Deliveries.Application.Dtos;
using Deliveries.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Deliveries.Data;

public class DeliveryPersonRentalsRepository : IDeliveryPersonRentalsRepository
{
    private readonly DeliveriesContext _context;
    private readonly ILogger<DeliveryPersonRentalsRepository> _logger;
    private readonly IMapper _mapper;

    public DeliveryPersonRentalsRepository(DeliveriesContext context, IMapper mapper, ILogger<DeliveryPersonRentalsRepository> logger) 
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
    }

    public async Task CreateAsync(DeliveryPersonRental personRental)
    {
        try
        {
            var deliveryRentalDb = _mapper.Map<DeliveryPersonRentalDb>(personRental);

            deliveryRentalDb.DeliveryPerson = null;
            
            _context.DeliveryPersonRentals.Add(deliveryRentalDb);
            await _context.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating delivery rental.");
            throw;
        }
    }

    public async Task UpdateAsync(DeliveryPersonRental deliveryRental)
    {
        try
        {
            var deliveryRentalDb = _mapper.Map<DeliveryPersonRentalDb>(deliveryRental);

            _context.DeliveryPersonRentals.Update(deliveryRentalDb);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating delivery rental.");
            throw;
        }
    }

    public async Task<IEnumerable<DeliveryPersonRental>> GetDeliveryPersonRentalsByDeliveryPersonIdAsync(Guid id)
    {
        try
        {
            var deliveryRental = await _context.DeliveryPersonRentals
                .Include(x => x.DeliveryPerson)
                .Where(x => x.DeliveryPerson.Id == id)
                .ToListAsync();

            return _mapper.Map<IEnumerable<DeliveryPersonRental>>(deliveryRental);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting delivery person rentals.");
            throw;
        }
    }

    public DeliveryPersonRental GetDeliveryPersonRentals(Guid id)
    {
        try
        {
            var deliveryRental = _context
                .DeliveryPersonRentals
                    .Include(x => x.DeliveryPerson)
                .FirstOrDefault(x => x.Id == id);

            return _mapper.Map<DeliveryPersonRental>(deliveryRental);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting delivery person rentals.");
            throw;
        }
    }
}
