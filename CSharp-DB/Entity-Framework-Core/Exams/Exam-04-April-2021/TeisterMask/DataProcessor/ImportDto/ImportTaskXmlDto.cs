using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ImportDto;

[XmlType("Task")]
public class ImportTaskXmlDto
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
    [Required]
    public string DueDate { get; set; }

    [XmlElement("ExecutionType")]
    [Required]
    public int ExecutionType { get; set; }

    [XmlElement("LabelType")]
    [Required]
    public int LabelType { get; set; }
}
