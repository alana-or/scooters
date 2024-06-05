using AutoBogus;
using Deliveries.Data.Entities;

namespace Deliveries.Data.Builders;

public class DeliveryPersonBuilder
{
    private DeliveryPersonDb delivery;

    public DeliveryPersonBuilder()
    {
        delivery = new AutoFaker<DeliveryPersonDb>()
            .RuleFor(m => m.Id, f => f.Random.Guid())
            .RuleFor(m => m.Name, f => f.Name.FirstName())
            .RuleFor(m => m.CNHImage, f => f.Random.String2(1, 10))
            .RuleFor(m => m.CNPJ, f => f.Random.String2(1, 10))
            .RuleFor(m => m.CNH, f => f.Random.String2(1, 10))
            .RuleFor(m => m.CNHType, "A")
            .RuleFor(m => m.Birth, f => f.Date.Past(18))
            .Generate();
    }

    public List<DeliveryPersonDb> Generate(int quant)
    {
        return new AutoFaker<DeliveryPersonDb>()
            .RuleFor(m => m.Id, f => f.Random.Guid())
            .RuleFor(m => m.Name, f => f.Name.FirstName())
            .RuleFor(m => m.CNHImage, f => f.Random.String2(1, 10))
            .RuleFor(m => m.CNPJ, f => f.Random.String2(1, 10))
            .RuleFor(m => m.CNH, f => f.Random.String2(1, 10))
            .RuleFor(m => m.CNHType, "A")
            .RuleFor(m => m.Birth, f => f.Date.Past(18))
            .Generate(quant);
    }

    public DeliveryPersonDb Build()
    {
        return delivery;
    }
}
