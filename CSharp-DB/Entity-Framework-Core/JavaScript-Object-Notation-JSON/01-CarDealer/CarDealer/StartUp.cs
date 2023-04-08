using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;

namespace CarDealer;

public class StartUp
{
    public static void Main()
    {
        CarDealerContext context = new CarDealerContext();
        //string inputJson = File.ReadAllText(@"../../../Datasets/suppliers.json");


        string result = GetCarsWithTheirListOfParts(context);
        Console.WriteLine(result);
    }

    public static string ImportSuppliers(CarDealerContext context, string inputJson)
    {
        ImportSupplierDto[] supplierDtos =
            JsonConvert.DeserializeObject<ImportSupplierDto[]>(inputJson);

        ICollection<Supplier> suppliers = new HashSet<Supplier>();

        foreach (var s in supplierDtos)
        {
            suppliers.Add(new Supplier()
            {
                Name = s.Name,
                IsImporter = s.IsImporter
            });
        }

        context.Suppliers.AddRange(suppliers);

        context.SaveChangesAsync();

        return $"Successfully imported {suppliers.Count}.";
    }
    public static string ImportParts(CarDealerContext context, string inputJson)
    {
        ImportPartDto[] partDtos =
            JsonConvert.DeserializeObject<ImportPartDto[]>(inputJson);

        ICollection<Part> parts = new HashSet<Part>();

        foreach (ImportPartDto p in partDtos.Where(p => p.SupplierId != null))
        {
            parts.Add(new Part()
            {
                Name = p.Name,
                Price = p.Price,
                Quantity = p.Quantity,
                SupplierId = p.SupplierId
            });
        }

        context.Parts.AddRange(parts);

        context.SaveChangesAsync();

        return $"Successfully imported {parts.Count}.";
    }
    public static string ImportCars(CarDealerContext context, string inputJson)
    {
        ImportCarDto[] CarDtos =
            JsonConvert.DeserializeObject<ImportCarDto[]>(inputJson);

        ICollection<Car> cars = new HashSet<Car>();

        foreach (ImportCarDto c in CarDtos)
        {
            Car car = new Car()
            {
                Make = c.Make,
                Model = c.Model,
                TravelledDistance = c.TravelledDistance,
            };
            cars.Add(car);

            foreach (var pc in c.PartsCars)
            {
                PartCar partCar = new PartCar
                {
                    CarId = car.Id,
                    PartId = pc.PartId
                };

                car.PartsCars.Add(partCar);
            }
        }

        context.Cars.AddRange(cars);

        context.SaveChangesAsync();

        return $"Successfully imported {cars.Count}.";
    }
    public static string ImportCustomers(CarDealerContext context, string inputJson)
    {
        ImportCustomerDto[] importCustomerDtos =
            JsonConvert.DeserializeObject<ImportCustomerDto[]>(inputJson);

        ICollection<Customer> customers = new HashSet<Customer>();

        foreach (ImportCustomerDto c in importCustomerDtos)
        {
            customers.Add(new Customer()
            {
                Name = c.Name,
                BirthDate = c.BirthDate,
                IsYoungDriver = c.IsYoungDriver
            });
        }

        context.Customers.AddRange(customers);

        context.SaveChangesAsync();

        return $"Successfully imported {customers.Count}.";
    }
    public static string ImportSales(CarDealerContext context, string inputJson)
    {
        ImportSaleDto[] importSaleDtos =
            JsonConvert.DeserializeObject<ImportSaleDto[]>(inputJson);

        ICollection<Sale> sales = new HashSet<Sale>();

        foreach (ImportSaleDto s in importSaleDtos)
        {
            sales.Add(new Sale()
            {
                CarId = s.CarId,
                CustomerId = s.CustomerId,
                Discount = s.Discount
            });
        }

        context.Sales.AddRange(sales);

        context.SaveChangesAsync();

        return $"Successfully imported {sales.Count}.";
    }
    public static string GetOrderedCustomers(CarDealerContext context)
    {
        var customers = context.Customers
            .AsNoTracking()
            .Select(c => new
            {
                c.Name,
                BirthDate = c.BirthDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                c.IsYoungDriver
            })
            .OrderBy(c => c.BirthDate)
            .ThenBy(c => c.IsYoungDriver)
            .ToArray();

        return JsonConvert.SerializeObject(
            customers,
            Formatting.Indented);
    }
    public static string GetCarsFromMakeToyota(CarDealerContext context)
    {
        var cars = context.Cars
            .AsNoTracking()
            .Select(c => new
            {
                c.Id,
                c.Make,
                c.Model,
                c.TravelledDistance
            })
            .Where(c => c.Make == "Toyota")
            .OrderBy(c => c.Model)
            .ThenBy(c => c.TravelledDistance)
            .ToArray();

        return JsonConvert.SerializeObject
            (
                cars,
                Formatting.Indented
            );
    }
    public static string GetLocalSuppliers(CarDealerContext context)
    {
        var suppliers = context.Suppliers
            .AsNoTracking()
            .Where(s => !s.IsImporter)
            .Select(s => new
            {
                s.Id,
                s.Name,
                PartsCount = s.Parts.Count
            })
            .ToArray();

        return JsonConvert.SerializeObject
            (
                suppliers,
                Formatting.Indented
            );
    }
    public static string GetCarsWithTheirListOfParts(CarDealerContext context)
    {
        var cars = context.Cars
    .Select(c => new
    {
        car = new
        {
            Make = c.Make,
            Model = c.Model,
            TravelledDistance = c.TravelledDistance
        },
        parts = c.PartsCars.Select(pc => new
        {
            Name = pc.Part.Name,
            Price = $"{pc.Part.Price:F2}"
        }).ToArray(),
    }).ToArray();

        var jsonFile = JsonConvert.SerializeObject(cars, Formatting.Indented);

        return jsonFile;
    }
}