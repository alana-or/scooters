using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Deliveries.Data.Entities;

[Table("deliveries_rental")]
public class DeliveriesRentalDb
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
}
