using AutoMapper;
using Deliveries.Api.Domain;
using Deliveries.Api.Models;
using Deliveries.Data.Entities;

namespace Deliveries.Api.Mappers;

public class DeliveriesMapper : Profile
{
    public DeliveriesMapper()
    {
        CreateMap<DeliveryPerson, DeliveryPersonDb>()
            .ReverseMap();

        CreateMap<DeliveryPersonRentalDb, DeliveryPersonRental>()
            .ReverseMap();

        CreateMap<DeliveryPersonModel, DeliveryPersonDb>()
            .ReverseMap();

        CreateMap<ScooterModel, DeliveryPersonDb>()
            .ReverseMap();

        CreateMap<RentalModel, DeliveryPersonRentalDb>()
            .ForPath(x => x.DeliveryPerson, opt => opt.MapFrom(s => s.DeliveryPerson))
            .ForPath(x => x.ScooterId, opt => opt.MapFrom(s => s.Scooter.Id))
            .ForPath(x => x.Year, opt => opt.MapFrom(s => s.Scooter.Year))
            .ForPath(x => x.Model, opt => opt.MapFrom(s => s.Scooter.Model))
            .ForPath(x => x.LicencePlate, opt => opt.MapFrom(s => s.Scooter.LicencePlate))
            .ReverseMap();
    }
}
