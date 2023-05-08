using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Theatre.Data.Models;

namespace Theatre.DataProcessor.ImportDto;

public class ImportTicketDto
{
    [Required]
    [Range(1.00, 100.00)]
    public decimal Price { get; set; }

    [Required]
    [Range(1, 10)]
    public sbyte RowNumber { get; set; }

    [Required]
    [ForeignKey(nameof(Play))]
    public int PlayId { get; set; }
}
