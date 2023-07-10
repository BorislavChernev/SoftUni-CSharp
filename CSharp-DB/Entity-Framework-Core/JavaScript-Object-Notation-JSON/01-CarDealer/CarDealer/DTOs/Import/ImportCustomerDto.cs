using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer.DTOs.Import;

public class ImportCustomerDto
{

    //"name": "Marcelle Griego",
    //"birthDate": "1990-10-04T00:00:00",
    //"isYoungDriver": true
    [JsonProperty("name")]
    public string Name { get; set; } = null!;
    [JsonProperty("birthDate")]
    public DateTime BirthDate { get; set; }
    [JsonProperty("isYoungDriver")]
    public bool IsYoungDriver { get; set; }
}
