namespace Deliveries.Api.Domain;

public class DeliveryPerson
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public string Photo { get; private set; }

    public DeliveryPerson(string name, string photo)
    {
        Id = new Guid();
        Name = name;
        Photo = photo;
    }
}
