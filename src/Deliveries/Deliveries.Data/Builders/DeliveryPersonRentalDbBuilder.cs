using AutoBogus;
using Deliveries.Application.Dtos;

namespace Deliveries.Data.Builders;

public class DeliveryPersonRentalDbBuilder
{
    private DeliveryPersonRentalDb delivery;

    public DeliveryPersonRentalDbBuilder()
    {
        delivery = new AutoFaker<DeliveryPersonRentalDb>()
            .RuleFor(m => m.Id, new Guid())
            .RuleFor(m => m.ScooterId, new Guid())
            .RuleFor(m => m.LicencePlate, f => f.Random.String2(1, 10))
            .RuleFor(m => m.Year, f => f.Random.Int(1960, 2030))
            .RuleFor(m => m.Model, f => f.Random.String2(1, 10))
            .RuleFor(m => m.DeliveryPerson, new DeliveryPersonDbBuilder().Build())
            .Generate();
    }

    public List<DeliveryPersonRentalDb> Generate(int quant)
    {
        return new AutoFaker<DeliveryPersonRentalDb>()
            .RuleFor(m => m.Id, new Guid())
            .RuleFor(m => m.ScooterId, new Guid())
            .RuleFor(m => m.LicencePlate, f => f.Random.String2(1, 10))
            .RuleFor(m => m.Year, f => f.Random.Int(1960, 2030))
            .RuleFor(m => m.Model, f => f.Random.String2(1, 10))
            .RuleFor(m => m.DeliveryPerson, new DeliveryPersonDbBuilder().Build())
            .Generate(quant);
    }

    public DeliveryPersonRentalDb Build()
    {
        return delivery;
    }
}
