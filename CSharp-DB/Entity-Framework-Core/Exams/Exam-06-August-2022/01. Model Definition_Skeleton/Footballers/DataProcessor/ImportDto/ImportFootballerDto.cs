using Footballers.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ImportDto;

[XmlType("Footballer")]
public class ImportFootballerDto
{
    [XmlElement("Name")]
    [MinLength(2)]
    [MaxLength(40)]
    public string Name { get; set; } = null!;

    [XmlElement("ContractStartDate")]
    public string ContractStartDate { get; set; }

    [XmlElement("ContractEndDate")]
    public string ContractEndDate { get; set; }

    [XmlElement("BestSkillType")]
    public int BestSkillType { get; set; }

    [XmlElement("PositionType")]
    public int PositionType { get; set; }
}
