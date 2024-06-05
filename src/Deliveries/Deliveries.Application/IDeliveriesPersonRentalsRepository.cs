using Deliveries.Domain;

namespace Deliveries.Application;

public interface IDeliveryPersonRentalsRepository
{
    public Task CreateAsync(DeliveryPersonRental deliveryRental);
    public Task UpdateAsync(DeliveryPersonRental deliveryRental);
    public Task<IEnumerable<DeliveryPersonRental>> GetDeliveryPersonRentalsAsync(Guid id);
}
