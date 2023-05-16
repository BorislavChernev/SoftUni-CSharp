using System.ComponentModel.DataAnnotations;

namespace Trucks.Data.Models;

public class Client
{
    public Client()
    {
        this.ClientsTrucks = new HashSet<ClientTruck>();
    }
    [Key]
    public int Id { get; set; }

    [Required]
    [MinLength(3)]
    [MaxLength(40)]
    public string Name { get; set; }

    [Required]
    [MinLength(2)]
    [MaxLength(40)]
    public string Nationality { get; set; }

    [Required]
    public string Type { get; set; }

    public ICollection<ClientTruck> ClientsTrucks { get; set; }
}
