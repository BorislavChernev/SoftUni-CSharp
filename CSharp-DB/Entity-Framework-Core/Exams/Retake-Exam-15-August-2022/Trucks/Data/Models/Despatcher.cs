using System.ComponentModel.DataAnnotations;

namespace Trucks.Data.Models;

public class Despatcher
{
    public Despatcher()
    {
        this.Trucks = new HashSet<Truck>();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [MinLength(2)]
    [MaxLength(40)]
    public string Name { get; set; }

    public string Position { get; set; }

    public ICollection<Truck> Trucks { get; set; }
}
