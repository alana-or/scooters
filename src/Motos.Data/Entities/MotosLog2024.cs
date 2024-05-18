using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Motto.Entities;

[Table("motosLog2024")]
public class MotosLog2024
{
   
    [Required]
    [MaxLength(1000)]
    [Column("message")]
    public string Message { get; }

    public MotosLog2024(string message)
    {
        Message = message;
    }
}
