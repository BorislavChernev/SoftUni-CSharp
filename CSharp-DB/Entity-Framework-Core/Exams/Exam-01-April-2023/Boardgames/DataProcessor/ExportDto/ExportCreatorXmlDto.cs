using Boardgames.Data.Models;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto;

[XmlType("Creator")]
public class ExportCreatorXmlDto
{
    [XmlElement("CreatorName")]
    public string CreatorName { get; set; }

    [XmlAttribute("BoardgamesCount")]
    public int BoardgamesCount { get; set; }

    [XmlArray("Boardgames")]
    public ExportBoardgameXmlDto[] Boardgames { get; set; }
}
