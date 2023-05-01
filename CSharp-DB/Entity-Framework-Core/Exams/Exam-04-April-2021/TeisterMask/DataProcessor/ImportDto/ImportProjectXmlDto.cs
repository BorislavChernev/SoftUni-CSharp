using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ImportDto;

[XmlType("Project")]
public class ImportProjectXmlDto
{
    [XmlElement("Name")]
    [Required]
    [MinLength(2)]
    [MaxLength(40)]
    public string Name { get; set; }

    [XmlElement("OpenDate")]
    [Required]
    public string OpenDate { get; set; }

    [XmlElement("DueDate")]
    public string? DueDate { get; set; }

    [XmlArray("Tasks")]
    public ImportTaskXmlDto[] Tasks { get; set; }
}
