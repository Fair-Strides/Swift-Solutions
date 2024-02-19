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

    [Column("EventName")]
    public string EventName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ViewedDate { get; set; }

    [Required]
    [Column("EventID")]
    [StringLength(255)]
    public string EventId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EventDate { get; set; }

    [Required]
    [StringLength(255)]
    public string EventDescription { get; set; }

    [Required]
    [StringLength(255)]
    public string EventLocation { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("EventHistories")]
    public virtual PgUser User { get; set; }
}
