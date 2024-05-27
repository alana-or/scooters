using AutoMapper;
using Motos.API.Domain;
using Motos.Data.Entities;

namespace Motos.API.Mappers;

public class MotoMapper : Profile
{
    public MotoMapper()
    {
        CreateMap<Moto, MotoDB>();
        CreateMap<MotoDB, Moto>();
    }
}
