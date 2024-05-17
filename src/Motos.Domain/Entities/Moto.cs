using System.ComponentModel.DataAnnotations;

namespace Motto.Entities;

public class Moto
{
    [Key]
    public int id { get; set; }

    [Required]
    public int ano { get; set; }

    [Required]
    [MaxLength(100)]
    public string modelo { get; set; }

    [Required]
    [MaxLength(20)]
    public string placa { get; set; }
}
