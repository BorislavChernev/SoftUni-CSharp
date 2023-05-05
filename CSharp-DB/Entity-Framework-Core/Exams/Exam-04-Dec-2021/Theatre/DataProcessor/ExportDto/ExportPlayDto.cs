using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Theatre.Data.Models;
using Theatre.Data.Models.Enums;

namespace Theatre.DataProcessor.ExportDto;

[XmlType("Play")]
public class ExportPlayDto
{
    [XmlAttribute("Title")]
    [Required]
    [MinLength(4)]
    [MaxLength(50)]
    public string Title { get; set; }

    [XmlAttribute("Duration")]
    [Required]
    public string Duration { get; set; }

    [XmlAttribute("Rating")]
    [Required]
    public string Rating { get; set; }

    [XmlAttribute("Genre")]
    [Required]
    public string Genre { get; set; }

    [XmlArray("Actors")]
    public Actor[] Actors { get; set; }
}
