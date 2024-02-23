using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PopNGo.Models;

[Table("EventHistory")]
public partial class EventHistory
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("UserID")]
    public int UserId { get; set; }

    [Column("EventID")]
    public int EventId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ViewedDate { get; set; }

    [ForeignKey("EventId")]
    [InverseProperty("EventHistories")]
    public virtual Event Event { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("EventHistories")]
    public virtual PgUser User { get; set; }
}
