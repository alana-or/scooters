using Motos.Data.Entities;

namespace Motos.Data;

public interface IMotosRepository
{
    public Task Create(Moto moto);
    public Task<IEnumerable<Moto>> GetMotoAsync(string? placa);
    Task SaveMoto2024Async(MotosLog2024 message);
    public Task Update(Moto moto);
    public Task<IEnumerable<MotosLog2024>> GetMoto2024Async();
}
