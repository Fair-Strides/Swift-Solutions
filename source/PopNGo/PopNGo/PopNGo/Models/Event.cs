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
    [Column("ApiEventID")]
    [StringLength(255)]
    public string ApiEventId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EventDate { get; set; }

    [Required]
    [StringLength(255)]
    public string EventName { get; set; }

    [Required]
    [StringLength(1000)]
    public string EventDescription { get; set; }

    [Required]
    [StringLength(255)]
    public string EventLocation { get; set; }

    [StringLength(255)]
    public string EventImage { get; set; }

    [InverseProperty("Event")]
    public virtual ICollection<EventHistory> EventHistories { get; set; } = new List<EventHistory>();

    [InverseProperty("Event")]
    public virtual ICollection<FavoriteEvent> FavoriteEvents { get; set; } = new List<FavoriteEvent>();
}
