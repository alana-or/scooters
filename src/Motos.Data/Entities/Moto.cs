﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Motos.Data;

[Table("motos")]
public class Moto
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
    
    public Moto()
    {
        
    }
}
