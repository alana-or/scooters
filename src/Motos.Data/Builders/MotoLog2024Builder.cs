using AutoBogus;
using Motos.Data.Entities;

namespace Motos.Data.Builders;

public class MotoLog2024Builder
{
    private MotosLog2024 _moto;

    public MotoLog2024Builder()
    {
        _moto = new AutoFaker<MotosLog2024>()
            .RuleFor(m => m.Id, f => 0)
            .RuleFor(m => m.Message, f => f.Random.String2(1, 100))
            .Generate();
    }

    public List<MotosLog2024> Generate(int quant)
    {
        return new AutoFaker<MotosLog2024>()
            .RuleFor(m => m.Id, f => 0)
            .RuleFor(m => m.Message, f => f.Random.String2(1, 100))
            .Generate(quant);
    }

    public MotosLog2024 Build()
    {
        return _moto;
    }
}
