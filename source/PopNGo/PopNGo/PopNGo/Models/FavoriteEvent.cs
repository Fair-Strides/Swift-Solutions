using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PopNGo.Models;

public partial class FavoriteEvent
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("UserID")]
    public int UserId { get; set; }

    [Column("EventID")]
    public int EventId { get; set; }

    [ForeignKey("EventId")]
    [InverseProperty("FavoriteEvents")]
    public virtual Event Event { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("FavoriteEvents")]
    public virtual PgUser User { get; set; }
}
