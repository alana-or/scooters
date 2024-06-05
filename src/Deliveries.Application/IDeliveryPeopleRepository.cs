using Deliveries.Domain;

namespace Deliveries.Application;
public interface IDeliveryPeopleRepository
{
    public Task CreateAsync(DeliveryPerson person);
    public Task UpdateAsync(DeliveryPerson person);
    public Task<DeliveryPerson> GetDeliveryPersonAsync(Guid guid);
}
