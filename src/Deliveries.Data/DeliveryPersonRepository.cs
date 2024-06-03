﻿using Bogus;
using Deliveries.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Deliveries.Data;

public interface IDeliveryPersonRepository
{
    public Task CreateAsync(DeliveryPersonDb person);
    public Task UpdateAsync(DeliveryPersonDb person);
    public Task<DeliveryPersonDb> GetDeliveryPersonAsync(Guid guid);
}

public class DeliveryPersonRepository : IDeliveryPersonRepository
{
    private readonly DeliveriesContext _context;
    private readonly ILogger<DeliveryPersonRepository> _logger;

    public DeliveryPersonRepository(DeliveriesContext context, 
        ILogger<DeliveryPersonRepository> logger)
    {
        _logger = logger;
        _context = context;
    }

    public async Task CreateAsync(DeliveryPersonDb person)
    {
        try
        {
            _context.DeliveryPeople.Add(person);
            await _context.SaveChangesAsync();
        }
        catch(Exception ex) 
        {
            _logger.LogError(ex, "An error occurred while creating delivery person.");
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
            _logger.LogError(ex, "An error occurred while updating delivery person.");
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
            _logger.LogError(ex, "An error occurred while getting the delivery person.");
            throw;
        }
    }
}
