using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto;

[XmlType("Boardgame")]
public class ExportBoardgameXmlDto
{
    [XmlElement("BoardgameName")]
    [Required]
    [MinLength(10)]
    [MaxLength(20)]
    public string BoardgameName { get; set; }

    [XmlElement("BoardgameYearPublished")]
    [Required]
    [Range(2018, 2023)]
    public int BoardgameYearPublished { get; set; }
}
