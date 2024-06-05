namespace Deliveries.Domain;

public class DeliveryPerson
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string CNHImage { get; private set; }
    public string CNPJ { get; private set; }
    public string CNH { get; private set; }
    public CNH CNHType { get; private set; }
    public DateTime Birth { get; private set; }

    public DeliveryPerson()
    {
    }

    public DeliveryPerson(string name, string cNHImage, string cNPJ, string cNH,
        char type, DateTime birth, Guid id = new Guid())
    {
        Id = id;
        Name = name;
        CNHImage = cNHImage;
        CNPJ = cNPJ;
        CNH = cNH;
        CNHType = type == 'A' ? Domain.CNH.A : Domain.CNH.B;
        Birth = birth.ToUniversalTime();
    }

    public void UpdateCNHImage(string image)
    {
        CNHImage = image;
    }
}
