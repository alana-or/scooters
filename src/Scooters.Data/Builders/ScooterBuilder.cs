using AutoBogus;
using Scooters.Data.Entities;

namespace Scooters.Data.Builders;

public class ScooterBuilder
{
    private ScooterDB scooter;

    public ScooterBuilder()
    {
        scooter = new AutoFaker<ScooterDB>()
            .RuleFor(m => m.Year, f => f.Random.Int(1960, 2030))
            .RuleFor(m => m.Id, f => 0)
            .RuleFor(m => m.LicencePlate, f => f.Random.String2(1, 20))
            .RuleFor(m => m.Model, f => f.Random.String2(1, 100))
            .Generate();
    }

    public List<ScooterDB> Generate(int quant)
    {
        return new AutoFaker<ScooterDB>()
            .RuleFor(m => m.Year, f => f.Random.Int(1960, 2030))
            .RuleFor(m => m.Id, f => 0)
            .RuleFor(m => m.LicencePlate, f => f.Random.String2(1, 20))
            .RuleFor(m => m.Model, f => f.Random.String2(1, 100))
            .Generate(quant);
    }

    public ScooterBuilder SetYear(int year)
    {
        scooter.Year = year;
        return this;
    }

    public ScooterBuilder SetModel(string model)
    {
        scooter.Model = model;
        return this;
    }

    public ScooterBuilder SetLicencePlate(string licence_plate)
    {
        scooter.LicencePlate = licence_plate;
        return this;
    }

    public ScooterDB Build()
    {
        return scooter;
    }
}
