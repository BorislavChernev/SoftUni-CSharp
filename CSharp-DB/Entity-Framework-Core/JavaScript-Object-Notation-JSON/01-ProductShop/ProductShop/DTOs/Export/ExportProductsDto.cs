using Newtonsoft.Json;
using ProductShop.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProductShop.DTOs.Export;

public class ExportProductsDto
{
    public ExportProductsDto()
    {
        this.ProductsSold = new HashSet<Product>();
    }
    [JsonProperty("firstName")]
    public string? FirstName { get; set; }

    [JsonProperty("lastName")]
    public string LastName { get; set; } = null!;

    [JsonProperty("soldProducts")]
    public virtual ICollection<Product> ProductsSold { get; set; }
}