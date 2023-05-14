using Footballers.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Footballers.DataProcessor.ImportDto;

public class ImportTeamDto
{
    [MinLength(3)]
    [MaxLength(40)]
    [RegularExpression(@"^[A-Za-z0-9\s\.\-]{3,}$")]
    public string Name { get; set; } = null!;

    [MinLength(2)]
    [MaxLength(40)]
    public string Nationality { get; set; } = null!;

    public int Trophies { get; set; }

    public int[] Footballers { get; set; }
}
