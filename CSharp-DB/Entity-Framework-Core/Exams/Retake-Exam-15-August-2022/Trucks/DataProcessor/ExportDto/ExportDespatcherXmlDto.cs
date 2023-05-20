using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Trucks.DataProcessor.ExportDto;

[XmlType("Despatcher")]
public class ExportDespatcherXmlDto
{
    [XmlElement("DespatcherName")]
    public string DespatcherName { get; set; }

    [XmlAttribute("TrucksCount")]
    public int TrucksCount { get; set; }

    [XmlArray("Trucks")]
    public ExportTruckXmlDto[] Trucks { get; set; }
}
