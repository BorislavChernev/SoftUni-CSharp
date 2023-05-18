using System.ComponentModel.DataAnnotations;
using Trucks.Data.Models;

namespace Trucks.DataProcessor.ExportDto;

public class ExportDespatcherDto
{
    [Required]
    [MinLength(2)]
    [MaxLength(40)]
    public string Name { get; set; }

    public ExportTruckDto[] Trucks { get; set; }
}
