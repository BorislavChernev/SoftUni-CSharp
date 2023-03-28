using System.ComponentModel.DataAnnotations;

namespace MusicHub.Data.Models;

public class Writer
{
    [Key]
    public int Id { get; set; }

    [MaxLength(ValidationConstants.WriterNameMaxLength)]
    public string Name { get; set; } = null!;

    public string? Pseudonym { get; set; }

    public virtual ICollection<Song>? Songs { get; set; }
}
