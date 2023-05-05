using System.ComponentModel.DataAnnotations;

namespace Theatre.Data.Models;

public class Theatre
{
    public Theatre()
    {
        this.Tickets = new HashSet<Ticket>();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [MinLength(4)]
    [MaxLength(30)]
    public string Name { get; set; }

    [Required]
    [Range(1, 10)]
    public sbyte NumberOfHalls { get; set; }

    [Required]
    [MinLength(4)]
    [MaxLength(30)]
    public string Director { get; set; }

    public ICollection<Ticket> Tickets { get; set; }
}
