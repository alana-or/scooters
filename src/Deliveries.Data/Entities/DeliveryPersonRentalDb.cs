using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Deliveries.Data.Entities;

[Table("deliveries_rental")]
public class DeliveryPersonRentalDb
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    
    [Required]
    [Column("scooter-id")]
    public Guid ScooterId { get; set; }

    [Required]
    [Column("year")]
    public int Year { get; set; }

    [Required]
    [Column("model")]
    public string Model { get; set; }

    [Required]
    [Column("licence-plate")]
    public string LicencePlate { get; set; }

    [Required]
    [ForeignKey("delivery-person-id")]
    public Guid DeliveryPersonId { get; set; }

    [Required]
    [Column("create")]
    public DateTime Create { get; set; }

    [Required]
    [Column("start")]
    public DateTime Start { get; set; }

    [Required]
    [Column("end")]
    public DateTime End { get; set; }

    [Required]
    [Column("expected-end")]
    public DateTime EndExpected { get; set; }

    public virtual DeliveryPersonDb DeliveryPerson { get; set; }
}
