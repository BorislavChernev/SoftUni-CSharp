using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer.DTOs.Import;

public class ImportCarDto
{
    [JsonProperty("make")]
    public string Make { get; set; } = null!;
    [JsonProperty("model")]
    public string Model { get; set; } = null!;
    [JsonProperty("traveledDistance")]
    public long TravelledDistance { get; set; }
    [JsonProperty("partsId")]
    public ICollection<PartCar> PartsCars { get; set; } = new List<PartCar>();
}
