using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Alquiler_videojuegos.Models;

[Table("client")]
public partial class Client
{
    [Key]
    [Column("id_client")]
    public int IdClient { get; set; }

    [Column("first_name")]
    [StringLength(30)]
    [Unicode(false)]
    public string FirstName { get; set; } = null!;

    [Column("last_name")]
    [StringLength(30)]
    [Unicode(false)]
    public string LastName { get; set; } = null!;

    [Column("age")]
    public int Age { get; set; }

    [Column("address")]
    [StringLength(100)]
    [Unicode(false)]
    public string Address { get; set; } = null!;

    [Column("birthday", TypeName = "date")]
    public DateTime Birthday { get; set; }

    [Column("gender")]
    [StringLength(30)]
    [Unicode(false)]
    public string Gender { get; set; } = null!;

    [Column("email")]
    [StringLength(50)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [InverseProperty("IdClientNavigation")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
