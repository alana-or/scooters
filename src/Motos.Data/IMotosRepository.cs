using Motto.Entities;

namespace Motos.Data;

public interface IMotosRepository
{
    public Task CreateMoto(Moto moto);
    public Task<IEnumerable<Moto>> GetMotoAsync(string? placa);
    Task SaveMoto2024(MotosLog2024 message);
    public Task UpdateMoto(Moto moto);

}
