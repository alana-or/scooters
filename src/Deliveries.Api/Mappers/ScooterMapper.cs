using AutoMapper;
using Deliveries.Api.Domain;
using Deliveries.Data.Entities;

namespace Deliveries.Api.Mappers;

public class ScooterMapper : Profile
{
    public ScooterMapper()
    {
        CreateMap<DeliveryPerson, DeliveryPersonDb>();
        CreateMap<DeliveryPersonDb, DeliveryPerson>();
    }
}
