using Newtonsoft.Json;

namespace CarDealer.DTOs.Import;

public class ImportSaleDto
{
    [JsonProperty("discount")]
    public decimal Discount { get; set; }
    [JsonProperty("carId")]
    public int CarId { get; set; }
    [JsonProperty("customerId")]
    public int CustomerId { get; set; }
}
