using AutoBogus;
using Deliveries.Domain;

namespace Deliveries.Data.Builders;

public class DeliveryPersonBuilder
{
    private DeliveryPerson delivery;

    public DeliveryPersonBuilder()
    {
        delivery = new AutoFaker<DeliveryPerson>()
            .RuleFor(m => m.Id, f => f.Random.Guid())
            .RuleFor(m => m.Name, f => f.Name.FirstName())
            .RuleFor(m => m.CNHImage, f => f.Random.String2(1, 10))
            .RuleFor(m => m.CNPJ, f => f.Random.String2(1, 10))
            .RuleFor(m => m.CNH, f => f.Random.String2(1, 10))
            .RuleFor(m => m.CNHType, CNH.A)
            .RuleFor(m => m.Birth, f => f.Date.Past(18))
            .Generate();
    }

    public List<DeliveryPerson> Generate(int quant)
    {
        return new AutoFaker<DeliveryPerson>()
            .RuleFor(m => m.Id, f => f.Random.Guid())
            .RuleFor(m => m.Name, f => f.Name.FirstName())
            .RuleFor(m => m.CNHImage, f => f.Random.String2(1, 10))
            .RuleFor(m => m.CNPJ, f => f.Random.String2(1, 10))
            .RuleFor(m => m.CNH, f => f.Random.String2(1, 10))
            .RuleFor(m => m.CNHType, CNH.A)
            .RuleFor(m => m.Birth, f => f.Date.Past(18))
            .Generate(quant);
    }

    public DeliveryPerson Build()
    {
        return delivery;
    }
}
