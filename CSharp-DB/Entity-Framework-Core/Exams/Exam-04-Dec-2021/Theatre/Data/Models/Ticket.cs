using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theatre.Data.Models;

public class Ticket
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Range(1.00, 100.00)]
    public decimal Price { get; set; }

    [Required]
    [Range(1, 10)]
    public sbyte RowNumber { get; set; }

    [Required]
    [ForeignKey(nameof(Play))]
    public int PlayId { get; set; }

    [Required]
    public Play Play { get; set; }

    [Required]
    [ForeignKey(nameof(Theatre))]
    public int TheatreId { get; set; }

    [Required]
    public Theatre Theatre { get; set; }
}
