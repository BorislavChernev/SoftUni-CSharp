using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ExportDto;

[XmlType("Gun")]
public class ExportGunXmlDto
{
    [XmlAttribute("Manufacturer")]
    [Required]
    public string Manufacturer { get; set; }

    [XmlAttribute("GunType")]
    [Required]
    public string GunType { get; set; }

    [XmlAttribute("GunWeight")]
    [Required]
    [MinLength(100)]
    [MaxLength(1_350_000)]
    public int GunWeight { get; set; }

    [XmlAttribute("BarrelLength")]
    [Required]
    [Range(2.00, 35.00)]
    public double BarrelLength { get; set; }

    [XmlAttribute("Range")]
    [Required]
    [Range(1, 100_000)]
    public int Range { get; set; }

    [XmlArray("Countries")]
    public ExportCountryXmlDto[] Countries { get; set; }
}
