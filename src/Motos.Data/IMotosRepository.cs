using Motto.Entities;

namespace Motos.Data;

public interface IMotosRepository
{
    public Task CreateMoto(Moto moto);
    public Task<IEnumerable<Moto>> GetMotoAsync(string? placa);
    public Task UpdateMoto(Moto moto);

}
