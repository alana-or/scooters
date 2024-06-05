using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Deliveries.Application.Dtos;

[Table("delivery_person")]
public class DeliveryPersonDb
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [StringLength(20)]
    [Column("name")]
    public string Name { get; set; }

    [Required]
    [StringLength(100)]
    [Column("cnh-image", TypeName = "varchar(100)")]
    public string CNHImage { get; set; }

    [Required]
    [StringLength(11)]
    [Column("cnpj", TypeName = "varchar(11)")]
    public string CNPJ { get; set; }

    [Required]
    [StringLength(11)]
    [Column("cnh", TypeName = "varchar(11)")]
    public string CNH { get; set; }

    [Required]
    [StringLength(1)]
    [Column("cnh-type", TypeName = "char(1)")]
    public string CNHType { get; set; }

    [Required]
    [Column("birth", TypeName = "timestamp without time zone")]
    public DateTime Birth { get; set; }
}
