using AutoMapper;
using Deliveries.Api.Domain;
using Deliveries.Api.Models;
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

        CreateMap<DeliveryPersonResponse, DeliveryPersonDb>();
        CreateMap<ScooterResponse, DeliveryPersonDb>();

        CreateMap<DeliveryPersonRentalDb, RentalResponse>()
            .ForPath(x => x.DeliveryPerson, opt => opt.MapFrom(s => s.DeliveryPerson))
            .ForPath(x => x.Scooter.Id, opt => opt.MapFrom(s => s.scooterId))
            .ForPath(x => x.Scooter.Year, opt => opt.MapFrom(s => s.Year))
            .ForPath(x => x.Scooter.Model, opt => opt.MapFrom(s => s.Model))
            .ForPath(x => x.Scooter.LicencePlate, opt => opt.MapFrom(s => s.LicencePlate));

        CreateMap<RentalResponse, DeliveryPersonRentalDb>()
            .ForPath(x => x.DeliveryPerson, opt => opt.MapFrom(s => s.DeliveryPerson))
            .ForPath(x => x.scooterId, opt => opt.MapFrom(s => s.Scooter.Id))
            .ForPath(x => x.Year, opt => opt.MapFrom(s => s.Scooter.Year))
            .ForPath(x => x.Model, opt => opt.MapFrom(s => s.Scooter.Model))
            .ForPath(x => x.LicencePlate, opt => opt.MapFrom(s => s.Scooter.LicencePlate));
    }
}
