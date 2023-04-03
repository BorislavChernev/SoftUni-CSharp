using Newtonsoft.Json;

namespace ProductShop.DTOs.Export;

public class ExportUserDto
{
    [JsonProperty("firstName")]
    public string? FirstName { get; set; }

    [JsonProperty("lastName")]
    public string LastName { get; set; } = null!;

    [JsonProperty("age")]
    public int? Age { get; set; }
}
