using AutoBogus;
using Deliveries.Data.Entities;

namespace Deliveries.Data.Builders;

public class DeliveryPersonBuilder
{
    private DeliveryPersonDb delivery;

    public DeliveryPersonBuilder()
    {
        delivery = new AutoFaker<DeliveryPersonDb>()
            .RuleFor(m => m.Id, new Guid())
            .RuleFor(m => m.Name, f => f.Random.String2(1, 100))
            .RuleFor(m => m.Photo, f => f.Random.String2(1, 100))
            .Generate();
    }

    public List<DeliveryPersonDb> Generate(int quant)
    {
        return new AutoFaker<DeliveryPersonDb>()
            .RuleFor(m => m.Id, new Guid())
            .RuleFor(m => m.Name, f => f.Random.String2(1, 100))
            .RuleFor(m => m.Photo, f => f.Random.String2(1, 100))
            .Generate(quant);
    }

    public DeliveryPersonBuilder SetName(string name)
    {
        delivery.Name = name;
        return this;
    }

    public DeliveryPersonBuilder SetPhoto(string photo)
    {
        delivery.Photo = photo;
        return this;
    }

    public DeliveryPersonDb Build()
    {
        return delivery;
    }
}
