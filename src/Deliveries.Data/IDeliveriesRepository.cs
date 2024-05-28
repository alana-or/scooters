using Deliveries.Data.Entities;

namespace Deliveries.Data;

public interface IDeliveriesRepository
{
    public Task CreateAsync(DeliveryPersonDb person);
    public Task UpdateAsync(DeliveryPersonDb person);
    public Task<IEnumerable<DeliveryPersonDb>> GetDeliveryPeopleAsync();
}
