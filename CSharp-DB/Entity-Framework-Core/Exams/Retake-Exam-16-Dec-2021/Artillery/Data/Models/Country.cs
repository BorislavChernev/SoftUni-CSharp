using System.ComponentModel.DataAnnotations;

namespace Artillery.Data.Models;

public class Country
{
    public Country()
    {
        this.CountriesGuns = new HashSet<CountryGun>();
    }
    [Key]
    public int Id { get; set; }

    [Required]
    [MinLength(4)]
    [MaxLength(60)]
    public string CountryName { get; set; }

    [Required]
    [Range(50_000, 10_000_000)]
    public int ArmySize { get; set; }

    public ICollection<CountryGun> CountriesGuns { get; set; }
}
