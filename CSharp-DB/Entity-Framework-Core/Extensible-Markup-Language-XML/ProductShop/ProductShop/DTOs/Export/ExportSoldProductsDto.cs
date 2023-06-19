using ProductShop.Models;
using System.Xml.Serialization;

namespace ProductShop.DTOs.Export;

[XmlType("User")]
public class ExportSoldProductsDto
{
    public ExportSoldProductsDto()
    {
        this.SoldProducts = new List<ExportProductDto>();
    }

    public int Id { get; set; }

    [XmlElement("firstName")]
    public string FirstName { get; set; } = null!;

    [XmlElement("lastName")]
    public string LastName { get; set; } = null!;

    [XmlElement("soldProducts")]
    public ICollection<ExportProductDto> SoldProducts { get; set; } = null!;
}
