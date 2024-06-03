using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scooters.Data.Entities;

[Table("scooterslog2024")]
public class ScooterLog2024
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(1000)]
    [Column("message")]
    public string Message { get; }

    public ScooterLog2024(string message)
    {
        Id = new Guid();
        Message = message;
    }
}
