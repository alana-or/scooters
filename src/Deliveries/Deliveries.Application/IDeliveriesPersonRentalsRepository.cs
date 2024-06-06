using Deliveries.Domain;

namespace Deliveries.Application;

public interface IDeliveryPersonRentalsRepository
{
    public Task CreateAsync(DeliveryPersonRental deliveryRental);
    public Task UpdateAsync(DeliveryPersonRental deliveryRental);
    public Task<IEnumerable<DeliveryPersonRental>> GetDeliveryPersonRentalsByDeliveryPersonIdAsync(Guid id);
    public DeliveryPersonRental GetDeliveryPersonRentals(Guid id);
}
