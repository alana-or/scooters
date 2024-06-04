using AutoBogus;
using Deliveries.Data.Entities;

namespace Deliveries.Data.Builders;

public class DeliveryPersonRentalsBuilder
{
    private DeliveryPersonRentalDb delivery;

    public DeliveryPersonRentalsBuilder()
    {
        delivery = new AutoFaker<DeliveryPersonRentalDb>()
            .RuleFor(m => m.Id, new Guid())
            .RuleFor(m => m.ScooterId, new Guid())
            .RuleFor(m => m.LicencePlate, f => f.Random.String2(1, 10))
            .RuleFor(m => m.Year, f => f.Random.Int(1960, 2030))
            .RuleFor(m => m.Model, f => f.Random.String2(1, 10))
            .RuleFor(m => m.DeliveryPerson, new DeliveryPersonBuilder().Build())
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
            .RuleFor(m => m.DeliveryPerson, new DeliveryPersonBuilder().Build())
            .Generate(quant);
    }

    public DeliveryPersonRentalDb Build()
    {
        return delivery;
    }
}
