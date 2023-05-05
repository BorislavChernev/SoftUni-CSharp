using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Theatre.DataProcessor.ExportDto;

[XmlType("Actor")]
public class Actor
{
    [XmlAttribute("FullName")]
    [Required]
    [MinLength(4)]
    [MaxLength(30)]
    public string FullName { get; set; }

    [XmlAttribute("IsMainCharacter")]
    [Required]
    public string MainCharacter { get; set; }
}
