namespace Trucks.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using Data;
    using Newtonsoft.Json;
    using Trucks.Data.Models;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ImportDto;
    using Trucks.Utilities;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";

        public static string ImportDespatcher(TrucksContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlHelper xmlHelper = new XmlHelper();

            ImportDespatcherDto[] importDespatcherDtos =
                xmlHelper.Deserialize<ImportDespatcherDto[]>(xmlString, "Despatchers");

            ICollection<Despatcher> despatchers = new List<Despatcher>();
            foreach (ImportDespatcherDto d in importDespatcherDtos.Distinct())
            {
                if (!IsValid(d) || string.IsNullOrEmpty(d.Position))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Despatcher despatcher = new Despatcher()
                {
                    Name = d.Name,
                    Position = d.Position
                };

                foreach (ImportTruckDto t in d.Trucks.Distinct())
                {
                    if (!IsValid(t) || string.IsNullOrEmpty(t.RegistrationNumber))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Truck truck = new Truck()
                    {
                        RegistrationNumber = t.RegistrationNumber,
                        VinNumber = t.VinNumber,
                        TankCapacity = t.TankCapacity,
                        CargoCapacity = t.CargoCapacity,
                        CategoryType = (CategoryType)t.CategoryType,
                        MakeType = (MakeType)t.MakeType,
                    };

                    despatcher.Trucks.Add(truck);
                }
                despatchers.Add(despatcher);
                sb.AppendLine(String.Format(SuccessfullyImportedDespatcher, despatcher.Name, despatcher.Trucks.Count));
            }
            context.Despatchers.AddRange(despatchers);
            context.SaveChanges();

            //Console.WriteLine(despatchers.Count + " " + despatchers.Sum(x => x.Trucks.Count));
            return sb.ToString().TrimEnd();
        }
        public static string ImportClient(TrucksContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportClientDto[] importClientDtos =
                JsonConvert.DeserializeObject<ImportClientDto[]>(jsonString);

            ICollection<Client> clients = new List<Client>();
            ICollection<int> existingTrucks = context.Trucks
                .Select(t => t.Id)
                .ToArray();
            foreach (ImportClientDto c in importClientDtos.Distinct())
            {
                //if (c.Name == "GLOU AVTO GmbH")
                //{
                //    Console.WriteLine();
                //}

                if (!IsValid(c) || c.Type == "usual")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Client client = new Client()
                {
                    Name = c.Name,
                    Nationality = c.Nationality,
                    Type = c.Type,
                };

                foreach (int id in c.Trucks.Distinct())
                {
                    if (!existingTrucks.Contains(id))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    ClientTruck clientTruck = new ClientTruck()
                    {
                        TruckId = id,
                        ClientId = client.Id,
                    };

                    client.ClientsTrucks.Add(clientTruck);
                }
                clients.Add(client);
                sb.AppendLine(String.Format(SuccessfullyImportedClient, client.Name, client.ClientsTrucks.Count));
            }
            context.Clients.AddRange(clients);
            context.SaveChanges();
           // Console.WriteLine(sb.ToString().TrimEnd());

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}