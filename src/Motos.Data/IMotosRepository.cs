using Motos.Data.Entities;

namespace Motos.Data;

public interface IMotosRepository
{
    public Task CreateMoto(Moto moto);
    public Task<IEnumerable<Moto>> GetMotoAsync(string? placa);
    Task SaveMoto2024Async(MotosLog2024 message);
    public Task UpdateMoto(Moto moto);
    public Task<IEnumerable<MotosLog2024>> GetMoto2024Async();
}
