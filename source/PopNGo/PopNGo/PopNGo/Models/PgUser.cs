using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PopNGo.Models;

[Table("PG_User")]
public partial class PgUser
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [Column("ASPNETUserID")]
    [StringLength(255)]
    public string AspnetuserId { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<EventHistory> EventHistories { get; set; } = new List<EventHistory>();
}
