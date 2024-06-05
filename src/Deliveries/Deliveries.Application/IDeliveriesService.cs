using Deliveries.Application.Models;

namespace Deliveries.Application;

public interface IDeliveriesService
{
    public Task<Response<DeliveryPersonModel>> CreatePersonAsync(DeliveryPersonCreateModel request);
    public Task<Response<DeliveryPersonModel>> UpdatePersonAsync(DeliveryPersonUpdateModel request);
    public Task<Response<IEnumerable<RentalModel>>> GetPersonRentalsAsync(Guid idPerson);
    public Task<Response<RentalModel>> CreateRentalAsync(DeliveryPersonRentalCreateModel request);
    public Task<Response<IEnumerable<ScooterModel>>> GetScootersAsync();
}
