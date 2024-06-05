using AutoBogus;
using Scooters.Data.Entities;

namespace Scooters.Data.Builders;

public class ScooterLog2024Builder
{
    private ScooterLog2024 scooterLog;

    public ScooterLog2024Builder()
    {
        scooterLog = new AutoFaker<ScooterLog2024>()
            .RuleFor(m => m.Id, new Guid())
            .RuleFor(m => m.Message, f => f.Random.String2(1, 100))
            .Generate();
    }

    public List<ScooterLog2024> Generate(int quant)
    {
        return new AutoFaker<ScooterLog2024>()
            .RuleFor(m => m.Id, new Guid())
            .RuleFor(m => m.Message, f => f.Random.String2(1, 100))
            .Generate(quant);
    }

    public ScooterLog2024 Build()
    {
        return scooterLog;
    }
}
