using ProductShop.Models;
using System.Xml.Serialization;

namespace ProductShop.DTOs.Import;

[XmlType("CategoryProduct")]
public class ImportCategoryProductDto
{
    [XmlElement("CategoryId")]
    public int CategoryId { get; set; }
    [XmlElement("ProductId")]
    public int ProductId { get; set; }
    //<CategoryProduct>
    //    <CategoryId>4</CategoryId>
    //    <ProductId>1</ProductId>
}
