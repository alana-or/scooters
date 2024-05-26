using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Motos.Data.Entities;

[Table("motoslog2024")]
public class MotosLog2024
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(1000)]
    [Column("message")]
    public string Message { get; }

    public MotosLog2024(string message)
    {
        Message = message;
    }
}
