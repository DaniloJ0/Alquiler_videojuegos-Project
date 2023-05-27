using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Alquiler_videojuegos.Models;

[Table("rent")]
public partial class Rent
{
    [Key]
    [Column("id_rent")]
    public int IdRent { get; set; }

    [Column("id_game")]
    public int IdGame { get; set; }

    [Column("id_user")]
    public int IdUser { get; set; }

    [Column("returned")]
    public bool Returned { get; set; }

    [Column("rented_date", TypeName = "date")]
    public DateTime RentedDate { get; set; }

    [Column("expiration_days")]
    public int ExpirationDays { get; set; }

    [ForeignKey("IdGame")]
    [InverseProperty("Rents")]
    public virtual Game IdGameNavigation { get; set; } = null!;

    [ForeignKey("IdUser")]
    [InverseProperty("Rents")]
    public virtual User IdUserNavigation { get; set; } = null!;
}
