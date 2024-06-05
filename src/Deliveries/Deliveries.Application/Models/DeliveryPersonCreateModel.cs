namespace Deliveries.Application.Models;

public class DeliveryPersonCreateModel
{
    public string Name { get; set; }
    public string CNHImage { get; set; }
    public string CNPJ { get; set; }
    public string CNH { get; set; }
    public char CNHType { get; set; }
    public DateTime Birth { get; set; }
}