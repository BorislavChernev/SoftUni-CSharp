using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Boardgames.Data.Models;

public class Seller
{
    public Seller()
    {
        this.BoardgamesSellers = new HashSet<BoardgameSeller>();
    }

    [Key]
    public int Id { get; set; }

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

    public ICollection<BoardgameSeller> BoardgamesSellers { get; set; }
}
