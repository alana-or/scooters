using AutoMapper;
using Scooters.Api.Domain;
using Scooters.Data.Entities;

namespace Scooters.Api.Mappers;

public class ScooterMapper : Profile
{
    public ScooterMapper()
    {
        CreateMap<Scooter, ScooterDB>();
        CreateMap<ScooterDB, Scooter>();
    }
}
