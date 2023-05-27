using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Alquiler_videojuegos.Models;

[Table("game")]
public partial class Game
{
    [Key]
    [Column("id_game")]
    public int IdGame { get; set; }

    [Column("name")]
    [StringLength(50)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("release_year")]
    public int ReleaseYear { get; set; }

    [Column("protagonist")]
    [StringLength(50)]
    [Unicode(false)]
    public string Protagonist { get; set; } = null!;

    [Column("director")]
    [StringLength(50)]
    [Unicode(false)]
    public string Director { get; set; } = null!;

    [Column("platform")]
    [StringLength(50)]
    [Unicode(false)]
    public string Platform { get; set; } = null!;

    [Column("rental_price")]
    public double RentalPrice { get; set; }

    [InverseProperty("IdGameNavigation")]
    public virtual ICollection<Rent> Rents { get; set; } = new List<Rent>();
}
