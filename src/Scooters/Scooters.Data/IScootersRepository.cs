using Scooters.Data.Entities;

namespace Scooters.Data;

public interface IScootersRepository
{
    public Task CreateAsync(ScooterDB moto);
    public Task<IEnumerable<ScooterDB>> GetScootersAsync(string? licence_plate);
    Task SaveScooter2024Async(ScooterLog2024 message);
    public Task UpdateAsync(ScooterDB moto);
    public Task<IEnumerable<ScooterLog2024>> GetScooters2024Async();
}
