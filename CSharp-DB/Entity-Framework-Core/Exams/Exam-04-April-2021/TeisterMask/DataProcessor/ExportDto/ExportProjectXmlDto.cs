using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ExportDto;

[XmlType("Project")]
public class ExportProjectXmlDto
{
    [XmlElement("ProjectName")]
    [Required]
    [MinLength(2)]
    [MaxLength(40)]
    public string ProjectName { get; set; }

    [XmlElement("HasEndDate")]
    public string HasEndDate { get; set; }

    [XmlAttribute("TasksCount")]
    public int TasksCount { get; set; }

    [XmlArray("Tasks")]
    public ExportTaskXmlDto[] Tasks { get; set; }
}
