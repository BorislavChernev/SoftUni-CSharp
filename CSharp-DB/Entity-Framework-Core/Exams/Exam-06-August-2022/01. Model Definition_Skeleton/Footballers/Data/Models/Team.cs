using System.ComponentModel.DataAnnotations;

namespace Footballers.Data.Models;

public class Team
{
    public Team()
    {
        this.TeamsFootballers = new HashSet<TeamFootballer>();
    }

    [Key]
    public int Id { get; set; }

    [MinLength(3)]
    [MaxLength(40)]
    [RegularExpression(@"[A-Z][a-z][\s][\.][\-]")]
    public string Name { get; set; } = null!;

    [MinLength(2)]
    [MaxLength(40)]
    public string Nationality { get; set; } = null!;

    public int Trophies { get; set; }

    public virtual ICollection<TeamFootballer> TeamsFootballers { get; set; }
}
