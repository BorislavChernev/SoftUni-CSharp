namespace Trucks.DataProcessor
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using Trucks.DataProcessor.ExportDto;
    using Trucks.Utilities;

    public class Serializer
    {
        public static string ExportDespatchersWithTheirTrucks(TrucksContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();

            ExportDespatcherXmlDto[] despatcherXmlDtos = context.Despatchers
                .Where(d => d.Trucks.Any())
                .Select(d => new ExportDespatcherXmlDto()
                {
                    DespatcherName = d.Name,
                    TrucksCount = d.Trucks.Count,
                    Trucks = d.Trucks.Select(t => new ExportTruckXmlDto()
                    {
                        RegistrationNumber = t.RegistrationNumber,
                        MakeType = t.MakeType.ToString()
                    })
                    .OrderBy(t => t.RegistrationNumber)
                    .ToArray()
                })
                .OrderByDescending(d => d.TrucksCount)
                .ThenBy(d => d.DespatcherName)
                .ToArray();

            return xmlHelper.Serialize(despatcherXmlDtos, "Despatchers");
        }

        public static string ExportClientsWithMostTrucks(TrucksContext context, int capacity)
        {
            var clients = context.Clients
                .Include(c => c.ClientsTrucks)
                .ThenInclude(ct => ct.Truck)
                .AsNoTracking()
                .ToArray()
                .Where(d => d.ClientsTrucks.Any(t => t.Truck.TankCapacity >= capacity))
                .Select(d => new
                {
                    Name = d.Name,
                    Trucks = d.ClientsTrucks
                    .Where(t => t.Truck.TankCapacity >= capacity)
                    .Select(t => new
                    {
                        TruckRegistrationNumber = t.Truck.RegistrationNumber,
                        VinNumber = t.Truck.VinNumber,
                        TankCapacity = t.Truck.TankCapacity,
                        CargoCapacity = t.Truck.CargoCapacity,  
                        CategoryType = t.Truck.CategoryType.ToString(),
                        MakeType = t.Truck.MakeType.ToString(),
                    })
                    .OrderBy(t => t.MakeType)
                    .ThenByDescending(t => t.CargoCapacity)
                    .ToArray()
                })
                .OrderByDescending(d => d.Trucks.Count())
                .ThenBy(d => d.Name)
                .Take(10)
                .ToArray();

            return JsonConvert.SerializeObject(clients, Formatting.Indented);
        }
    }
}
