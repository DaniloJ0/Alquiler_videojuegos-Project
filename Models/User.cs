using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Alquiler_videojuegos.Models;

[Table("user")]
public partial class User
{
    [Key]
    [Column("id_user")]
    public int IdUser { get; set; }

    [Column("id_client")]
    public int IdClient { get; set; }

    [Column("admin")]
    public bool Admin { get; set; }

    [Column("password")]
    [StringLength(50)]
    [Unicode(false)]
    public string Password { get; set; } = null!;

    [ForeignKey("IdClient")]
    [InverseProperty("Users")]
    public virtual Client? IdClientNavigation { get; set; }

    [InverseProperty("IdUserNavigation")]
    public virtual ICollection<Rent> Rents { get; set; } = new List<Rent>();
}
