using AutoBogus;
using Bogus;
using Deliveries.Domain;

namespace Deliveries.Data.Builders;

public class DeliveryPersonRentalBuilder
{
    private DeliveryPersonRental delivery;

    public DeliveryPersonRentalBuilder()
    {
        delivery = new AutoFaker<DeliveryPersonRental>()
            .RuleFor(m => m.Id, new Guid())
            .RuleFor(m => m.ScooterId, new Guid())
            .RuleFor(m => m.LicencePlate, f => f.Random.String2(1, 10))
            .RuleFor(m => m.Year, f => f.Random.Int(1960, 2030))
            .RuleFor(m => m.Model, f => f.Random.String2(1, 10))
            .RuleFor(m => m.DeliveryPerson, new DeliveryPersonBuilder().Build())
            .Generate();
    }

    public List<DeliveryPersonRental> Generate(int quant)
    {
        return new AutoFaker<DeliveryPersonRental>()
            .RuleFor(m => m.Id, new Guid())
            .RuleFor(m => m.ScooterId, new Guid())
            .RuleFor(m => m.LicencePlate, f => f.Random.String2(1, 10))
            .RuleFor(m => m.Year, f => f.Random.Int(1960, 2030))
            .RuleFor(m => m.Model, f => f.Random.String2(1, 10))
            .RuleFor(m => m.DeliveryPerson, new DeliveryPersonBuilder().Build())
            .Generate(quant);
    }

    public DeliveryPersonRental Build()
    {
        return delivery;
    }

    public DeliveryPersonRentalBuilder WithPerson(DeliveryPerson person)
    {
        this.delivery.DeliveryPerson = person;
        return this;
    }

    public DeliveryPersonRentalBuilder WithLicencePlace(string licence)
    {
        this.delivery.LicencePlate = licence;
        return this;
    }

    public DeliveryPersonRentalBuilder WithModel(string model)
    {
        this.delivery.Model = model;
        return this;
    }

    public DeliveryPersonRentalBuilder WithYear(int year)
    {
        this.delivery.Year = year;
        return this;
    }
}
