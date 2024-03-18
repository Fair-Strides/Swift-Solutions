using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PopNGo.Models;

[Table("Tag")]
public partial class Tag
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(255)]
    public string Name { get; set; }

    [Required]
    [StringLength(255)]
    public string BackgroundColor { get; set; }

    [Required]
    [StringLength(255)]
    public string TextColor { get; set; }
}
