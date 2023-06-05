using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ExportDto;

[XmlType("Country")]
public class ExportCountryXmlDto
{
    [XmlAttribute("CountryName")]
    [Required]
    [MinLength(4)]
    [MaxLength(60)]
    public string CountryName { get; set; }

    [XmlAttribute("ArmySize")]
    [Required]
    [Range(50_000, 10_000_000)]
    public int ArmySize { get; set; }
}
