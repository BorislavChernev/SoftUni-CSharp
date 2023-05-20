using System.ComponentModel.DataAnnotations;
using Trucks.Data.Models.Enums;

namespace Trucks.DataProcessor.ExportDto;

public class ExportTruckDto
{
    [RegularExpression(@"^[A-Z]{2}\d{4}[A-Z]{2}$")]
    public string TruckRegistrationNumber { get; set; }

    [Required]
    [StringLength(17)]
    public string VinNumber { get; set; }

    [Range(950, 1420)]
    public int TankCapacity { get; set; }

    [Range(5000, 29000)]
    public int CargoCapacity { get; set; }

    [Required]
    public string CategoryType { get; set; }

    [Required]
    public string MakeType { get; set; }
}
