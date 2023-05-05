namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;
    using Trucks.Utilities;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";



        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlHelper xmlHelper = new XmlHelper();

            ImportPlayDto[] dtos =
                xmlHelper.Deserialize<ImportPlayDto[]>(xmlString, "Plays");

            string[] possible = new string[] { "Drama", "Comedy", "Romance", "Musical" };

            ICollection<Play> plays = new List<Play>();
            foreach (ImportPlayDto p in dtos)
            {
                if (!IsValid(p) || TimeSpan.ParseExact(p.Duration, "c", CultureInfo.InvariantCulture).Hours < 1 || !possible.Contains(p.Genre))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Play play = new Play()
                {
                    Title = p.Title,
                    Duration = TimeSpan.ParseExact(p.Duration, "c", CultureInfo.InvariantCulture),
                    Rating = p.Rating,
                    Genre = (Genre)Enum.Parse(typeof(Genre), p.Genre),
                    Description = p.Description,
                    Screenwriter = p.Screenwriter,
                };

                sb.AppendLine(String.Format(SuccessfulImportPlay, p.Title, p.Genre, p.Rating));
                plays.Add(play);
            }
            context.Plays.AddRange(plays);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlHelper xmlHelper = new XmlHelper();

            ImportCastDto[] dtos =
                xmlHelper.Deserialize<ImportCastDto[]>(xmlString, "Casts");

            ICollection<Cast> casts = new List<Cast>();
            foreach (ImportCastDto c in dtos)
            {
                if (!IsValid(c))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Cast cast = new Cast()
                {
                    FullName = c.FullName,
                    IsMainCharacter = c.IsMainCharacter == "false" ? false : true,
                    PhoneNumber = c.PhoneNumber,
                    PlayId = c.PlayId,
                };

                sb.AppendLine(String.Format(SuccessfulImportActor, c.FullName, c.IsMainCharacter == "false" ? "lesser" : "main"));
                casts.Add(cast);
            }
            context.Casts.AddRange(casts);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportTheatreDto[] dtos =
                JsonConvert.DeserializeObject<ImportTheatreDto[]>(jsonString);

            ICollection<Theatre> theatres = new List<Theatre>();
            foreach (ImportTheatreDto d in dtos)
            {
                if (!IsValid(d))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Theatre theatre = new Theatre()
                {
                    Name = d.Name,
                    NumberOfHalls = d.NumberOfHalls,
                    Director = d.Director,
                };

                foreach (ImportTicketDto t in d.Tickets)
                {
                    if (!IsValid(t))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Play current = context.Plays.Find(t.PlayId);

                    Ticket ticket = new Ticket()
                    {
                        Price = t.Price,
                        RowNumber = t.RowNumber,
                        PlayId = t.PlayId,
                        Play = current,
                    };

                    theatre.Tickets.Add(ticket);
                }
                sb.AppendLine(String.Format(SuccessfulImportTheatre, d.Name, theatre.Tickets.Count));
                theatres.Add(theatre);
            }
            context.Theatres.AddRange(theatres);
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
