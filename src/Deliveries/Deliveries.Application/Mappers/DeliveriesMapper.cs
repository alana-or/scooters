using AutoMapper;
using Deliveries.Application.Models;
using Deliveries.Application.Dtos;
using Deliveries.Domain;

namespace Deliveries.Application.Mappers;

public class DeliveriesMapper : Profile
{
    public DeliveriesMapper()
    {
        CreateMap<DeliveryPersonDb, DeliveryPerson>()
            .ReverseMap();

        CreateMap<DeliveryPersonRental, DeliveryPersonRentalDb>()
            .ReverseMap();

        CreateMap<DeliveryPersonRental, RentalReturnedModel>()
            .ReverseMap();

        CreateMap<DeliveryPersonModel, DeliveryPerson>()
            .ForMember(dest => dest.CNHType, opt => opt.MapFrom(src => src.CNHType == 'A' ? CNH.A : CNH.B))
            .ReverseMap()
            .ForMember(dest => dest.CNHType, opt => opt.MapFrom(src => (src.CNHType == CNH.A) ? 'A': 'B'));

        CreateMap<RentalModel, DeliveryPersonRental>()
            .ForPath(x => x.DeliveryPerson, opt => opt.MapFrom(s => s.DeliveryPerson))
            .ForPath(x => x.ScooterId, opt => opt.MapFrom(s => s.Scooter.Id))
            .ForPath(x => x.Year, opt => opt.MapFrom(s => s.Scooter.Year))
            .ForPath(x => x.Model, opt => opt.MapFrom(s => s.Scooter.Model))
            .ForPath(x => x.LicencePlate, opt => opt.MapFrom(s => s.Scooter.LicencePlate))
            .ReverseMap();
    }
}
