﻿namespace MusicHub.Data.Models;

using MusicHub.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Song
{
    public Song()
    {
        this.SongPerformers = new HashSet<SongPerformer>();
    }

    [Key]
    public int Id { get; set; }

    [MaxLength(ValidationConstants.SongNameMaxLength)]
    public string Name { get; set; } = null!;

    public TimeSpan Duration { get; set; }

    public DateTime CreatedOn { get; set; }

    public Genre Genre { get; set; }

    [ForeignKey(nameof(Album))]
    public int? AlbumId { get; set; }

    public virtual Album? Album { get; set; }

    [ForeignKey(nameof(Writer))]
    public int WriterId { get; set; }

    public virtual Writer Writer { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual ICollection<SongPerformer> SongPerformers { get; set; }
}
