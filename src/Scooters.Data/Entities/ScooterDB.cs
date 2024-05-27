using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scooters.Data.Entities;

[Table("scooters")]
public class ScooterDB
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("year")]
    public int Year { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("model")]
    public string Model { get; set; }

    [Required]
    [MaxLength(20)]
    [Column("licence_plate")]
    public string LicencePlate { get; set; }
    
    public ScooterDB()
    {
        
    }
}
