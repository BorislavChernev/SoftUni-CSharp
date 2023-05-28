using System.ComponentModel.DataAnnotations;

namespace Artillery.Data.Models;

public class Shell
{
    public Shell()
    {
        this.Guns = new HashSet<Gun>();
    }
    [Key]
    public int Id { get; set; }

    [Required]
    [Range(2, 1_680)]
    public double ShellWeight { get; set; }

    [Required]
    [MinLength(4)]
    [MaxLength(30)]
    public string Caliber { get; set; }

    public ICollection<Gun> Guns { get; set; }
}
