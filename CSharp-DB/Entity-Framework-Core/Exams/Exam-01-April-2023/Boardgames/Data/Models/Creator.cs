using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Boardgames.Data.Models;

public class Creator
{
    public Creator()
    {
        this.Boardgames = new HashSet<Boardgame>();
    }
    [Key]
    public int Id { get; set; }

    [Required]
    [MinLength(2)]
    [MaxLength(7)]
    public string FirstName { get; set; }

    [Required]
    [MinLength(2)]
    [MaxLength(7)]
    public string LastName { get; set; }

    public ICollection<Boardgame> Boardgames { get; set; }
}
