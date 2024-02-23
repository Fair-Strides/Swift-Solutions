using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PopNGo.Models;

[Table("Event")]
public partial class Event
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [Column("EventID")]
    [StringLength(255)]
    public string EventId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EventDate { get; set; }

    [Required]
    [StringLength(255)]
    public string EventName { get; set; }

    [Required]
    [StringLength(255)]
    public string EventDescription { get; set; }

    [Required]
    [StringLength(255)]
    public string EventLocation { get; set; }

    [InverseProperty("Event")]
    public virtual ICollection<EventHistory> EventHistories { get; set; } = new List<EventHistory>();
}
