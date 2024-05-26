namespace Motos.API.Models;

public class CreateMotoRequest
{
    public int Ano { get; set; }
    public string Modelo { get; set; }
    public string Placa { get; set; }
}