using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto;

[XmlType("Shell")]
public class ImportShellDto
{
    [XmlElement("ShellWeight")]
    [Required]
    [Range(2, 1_680)]
    public double ShellWeight { get; set; }

    [XmlElement("Caliber")]
    [Required]
    [MinLength(4)]
    [MaxLength(30)]
    public string Caliber { get; set; }
}
