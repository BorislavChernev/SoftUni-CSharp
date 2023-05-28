namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ImportDto;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using Trucks.Utilities;

    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlHelper xmlHelper = new XmlHelper();

            ImportCountryDto[] importCountryDtos =
                xmlHelper.Deserialize<ImportCountryDto[]>(xmlString, "Countries");

            ICollection<Country> countries = new List<Country>();
            foreach (ImportCountryDto c in importCountryDtos)
            {
                if (!IsValid(c))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Country country = new Country()
                {
                    CountryName = c.CountryName,
                    ArmySize = c.ArmySize,
                };

                countries.Add(country);
                sb.AppendLine(String.Format(SuccessfulImportCountry, country.CountryName, country.ArmySize));
            }
            context.Countries.AddRange(countries);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlHelper xmlHelper = new XmlHelper();

            ImportManufacturerDto[] importManufacturerDtos =
                xmlHelper.Deserialize<ImportManufacturerDto[]>(xmlString, "Manufacturers");

            ICollection<Manufacturer> manufacturers = new List<Manufacturer>();
            foreach (ImportManufacturerDto m in importManufacturerDtos.Distinct())
            {
                if (!IsValid(m) || manufacturers.Any(e => e.ManufacturerName == m.ManufacturerName))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Manufacturer manufacturer = new Manufacturer()
                {
                    ManufacturerName = m.ManufacturerName,
                    Founded = m.Founded,
                };
                string[] cmdArgs = manufacturer.Founded.Split(", ", StringSplitOptions.RemoveEmptyEntries);
                string townName = cmdArgs[cmdArgs.Length - 2];
                string countryName = cmdArgs[cmdArgs.Length - 1];

                manufacturers.Add(manufacturer);
                sb.AppendLine(String.Format(SuccessfulImportManufacturer, manufacturer.ManufacturerName, townName + ", " + countryName));
            }
            context.Manufacturers.AddRange(manufacturers);

            context.SaveChanges();
            Console.WriteLine(manufacturers.Count);

            return sb.ToString().TrimEnd();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlHelper xmlHelper = new XmlHelper();

            ImportShellDto[] Dtos =
                xmlHelper.Deserialize<ImportShellDto[]>(xmlString, "Shells");

            ICollection<Shell> shells = new List<Shell>();
            foreach (ImportShellDto x in Dtos)
            {
                if (!IsValid(x))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Shell shell = new Shell()
                {
                    ShellWeight = x.ShellWeight,
                    Caliber = x.Caliber,
                };

                shells.Add(shell);
                sb.AppendLine(String.Format(SuccessfulImportShell, shell.Caliber, shell.ShellWeight));
            }
            context.Shells.AddRange(shells);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportGunDto[] dtos =
                JsonConvert.DeserializeObject<ImportGunDto[]>(jsonString);

            ICollection<Gun> guns = new List<Gun>();
            foreach (ImportGunDto g in dtos)
            {
                if (!IsValid(g) || g.GunType == "InvalidFieldGun" || g.GunType == "InvalidMortar" || g.GunType == "nullFieldGun")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Gun gun = new Gun()
                {
                    ManufacturerId = g.ManufacturerId,
                    GunWeight = g.GunWeight,
                    BarrelLength = g.BarrelLength,
                    NumberBuild = g.NumberBuild,
                    Range = g.Range,
                    GunType = (GunType)Enum.Parse(typeof(GunType), g.GunType),
                    ShellId = g.ShellId,
                };

                foreach (var dto in g.Countries)
                {
                    gun.CountriesGuns.Add(new CountryGun()
                    {
                        CountryId = dto.Id,
                        Gun = gun,
                    });
                }
                guns.Add(gun);
                sb.AppendLine(String.Format(SuccessfulImportGun, g.GunType, g.GunWeight, g.BarrelLength));
            }
            context.Guns.AddRange(guns);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}