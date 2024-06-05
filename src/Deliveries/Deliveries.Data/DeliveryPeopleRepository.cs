using AutoMapper;
using Deliveries.Application;
using Deliveries.Application.Dtos;
using Deliveries.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Deliveries.Data;

public class DeliveryPeopleRepository : IDeliveryPeopleRepository
{
    private readonly DeliveriesContext _context;
    private readonly ILogger<DeliveryPeopleRepository> _logger;
    private readonly IMapper _mapper;

    public DeliveryPeopleRepository(DeliveriesContext context, 
        ILogger<DeliveryPeopleRepository> logger,
        IMapper mapper)
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
    }

    public async Task CreateAsync(DeliveryPerson person)
    {
        try
        {
            var deliveryPersonDB = _mapper.Map<DeliveryPersonDb>(person);

            _context.DeliveryPeople.Add(deliveryPersonDB);
            await _context.SaveChangesAsync();
        }
        catch(Exception ex) 
        {
            _logger.LogError(ex, "An error occurred while creating delivery person.");
            throw;
        }
    }

    public async Task UpdateAsync(DeliveryPerson person)
    {
        try
        {
            var deliveryPersonDB = _mapper.Map<DeliveryPersonDb>(person);

            _context.DeliveryPeople.Update(deliveryPersonDB);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "An error occurred while updating delivery person.");
            throw;
        }
    }

    public async Task<DeliveryPerson> GetDeliveryPersonAsync(Guid personId)
    {
        try
        {
            var person = await _context
                .DeliveryPeople
                .AsNoTracking()
                .FirstAsync(x => x.Id == personId);

            return _mapper.Map<DeliveryPerson>(person); 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting the delivery person.");
            throw;
        }
    }
}
