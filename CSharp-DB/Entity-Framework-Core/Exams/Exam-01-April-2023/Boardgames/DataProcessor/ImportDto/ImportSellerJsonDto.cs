using Boardgames.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Boardgames.DataProcessor.ImportDto;

public class ImportSellerJsonDto
{
    [Required]
    [MinLength(5)]
    [MaxLength(20)]
    public string Name { get; set; }

    [Required]
    [MinLength(2)]
    [MaxLength(30)]
    public string Address { get; set; }

    [Required]
    public string Country { get; set; }

    [Required]
    [RegularExpression(@"^www.[a-zA-z\d\-]+.com$")]
    public string Website { get; set; }

    public int[] Boardgames { get; set; }
}
