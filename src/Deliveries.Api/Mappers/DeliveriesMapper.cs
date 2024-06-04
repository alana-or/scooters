using AutoMapper;
using Deliveries.Api.Domain;
using Deliveries.Data.Entities;

namespace Deliveries.Api.Mappers;

public class DeliveriesMapper : Profile
{
    public DeliveriesMapper()
    {
        CreateMap<DeliveryPerson, DeliveryPersonDb>();
        CreateMap<DeliveryPersonDb, DeliveryPerson>();

        CreateMap<DeliveryPersonRentalDb, DeliveryPersonRental>();
        CreateMap<DeliveryPersonRental, DeliveryPersonRentalDb>();
    }
}
