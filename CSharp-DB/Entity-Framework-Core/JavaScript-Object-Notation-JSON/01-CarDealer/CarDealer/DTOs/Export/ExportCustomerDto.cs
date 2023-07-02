namespace CarDealer.DTOs.Export;

public class ExportCustomerDto
{
    public string Name { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public bool IsYoungDriver { get; set; }
}
