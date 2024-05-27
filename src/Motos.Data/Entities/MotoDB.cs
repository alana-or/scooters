using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Motos.Data.Entities;

[Table("motos")]
public class MotoDB
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("ano")]
    public int Ano { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("modelo")]
    public string Modelo { get; set; }

    [Required]
    [MaxLength(20)]
    [Column("placa")]
    public string Placa { get; set; }
    
    public MotoDB()
    {
        
    }
}
