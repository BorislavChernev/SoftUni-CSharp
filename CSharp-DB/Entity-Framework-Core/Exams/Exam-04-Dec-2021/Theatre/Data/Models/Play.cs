﻿using System.ComponentModel.DataAnnotations;
using Theatre.Data.Models.Enums;

namespace Theatre.Data.Models;

public class Play
{
    public Play()
    {
        this.Casts = new HashSet<Cast>();
        this.Tickets = new HashSet<Ticket>();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [MinLength(4)]
    [MaxLength(50)]
    public string Title { get; set; }

    [Required]
    public TimeSpan Duration { get; set; }

    [Required]
    [Range(0.00, 10.00)]
    public float Rating { get; set; }

    [Required]
    public Genre Genre { get; set; }

    [Required]
    [MaxLength(700)]
    public string Description { get; set; }

    [Required]
    [MinLength(4)]
    [MaxLength(30)]
    public string Screenwriter { get; set; }

    public ICollection<Cast> Casts { get; set; }

    public ICollection<Ticket> Tickets { get; set; }
}
