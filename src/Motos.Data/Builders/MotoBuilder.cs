using AutoBogus;
using Motos.Data.Entities;

namespace Motos.Data.Builders;

public class MotoBuilder
{
    private MotoDB _moto;

    public MotoBuilder()
    {
        _moto = new AutoFaker<MotoDB>()
            .RuleFor(m => m.Ano, f => f.Random.Int(1960, 2030))
            .RuleFor(m => m.Id, f => 0)
            .RuleFor(m => m.Placa, f => f.Random.String2(1, 20))
            .RuleFor(m => m.Modelo, f => f.Random.String2(1, 100))
            .Generate();
    }

    public List<MotoDB> Generate(int quant)
    {
        return new AutoFaker<MotoDB>()
            .RuleFor(m => m.Ano, f => f.Random.Int(1960, 2030))
            .RuleFor(m => m.Id, f => 0)
            .RuleFor(m => m.Placa, f => f.Random.String2(1, 20))
            .RuleFor(m => m.Modelo, f => f.Random.String2(1, 100))
            .Generate(quant);
    }

    public MotoBuilder SetAno(int ano)
    {
        _moto.Ano = ano;
        return this;
    }

    public MotoBuilder SetModelo(string modelo)
    {
        _moto.Modelo = modelo;
        return this;
    }

    public MotoBuilder SetPlaca(string placa)
    {
        _moto.Placa = placa;
        return this;
    }

    public MotoDB Build()
    {
        return _moto;
    }
}
