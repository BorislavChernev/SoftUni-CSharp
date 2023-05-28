using Artillery.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artillery.Data.Models;

public class Gun
{
    public Gun()
    {
        this.CountriesGuns = new HashSet<CountryGun>();
    }
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey(nameof(Manufacturer))]
    public int ManufacturerId { get; set; }

    [Required]
    public Manufacturer Manufacturer { get; set; }

    [Required]
    [MinLength(100)]
    [MaxLength(1_350_000)]
    public int GunWeight { get; set; }

    [Required]
    [Range(2.00, 35.00)]
    public double BarrelLength { get; set; }

    public int? NumberBuild { get; set; }

    [Required]
    [Range(1, 100_000)]
    public int Range { get; set; }

    [Required]
    public GunType GunType { get; set; }

    [Required]
    [ForeignKey(nameof(Shell))]
    public int ShellId { get; set; }

    [Required]
    public Shell Shell { get; set; }

    public ICollection<CountryGun> CountriesGuns { get; set; }
}
