using Deliveries.Data.Entities;

namespace Deliveries.Data;

public interface IDeliveriesRepository
{
    public Task CreateAsync(DeliveryPersonDb person);
    public Task UpdateAsync(DeliveryPersonDb person);
    public Task<IEnumerable<DeliveryPersonDb>> GetDeliveryPersonAsync();
    public Task<IEnumerable<DeliveryPersonRentalDb>> GetRentals(Guid personId);
    public Task CreateDeliveryRentalAsync(DeliveryPersonRentalDb deliveryRentalDb);
}
